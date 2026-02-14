#!/usr/bin/env python3
"""
Unity asset extractor for Melatonin.

Exports selected asset types from Unity data files using UnityPy and writes:
- one metadata record per extracted object (JSONL)
- summary counts by source file and type
- extracted files for supported object types
"""

from __future__ import annotations

import argparse
import json
import re
import sys
from collections import Counter
from pathlib import Path
from typing import Any

import UnityPy


DEFAULT_DATA_DIR = r"D:\games\steam\steamapps\common\Melatonin\Melatonin_Data"
DEFAULT_FILE_PATTERN = r"^(level\d+|sharedassets\d+\.assets|resources\.assets|globalgamemanagers)$"
DEFAULT_TYPES = ("AudioClip", "AnimationClip", "Sprite", "Texture2D", "TextAsset")
SAFE_NAME_PATTERN = re.compile(r"[^A-Za-z0-9._-]+")


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Extract selected Unity assets from Melatonin_Data."
    )
    parser.add_argument(
        "--data-dir",
        default=DEFAULT_DATA_DIR,
        help=f"Path to game data directory (default: {DEFAULT_DATA_DIR})",
    )
    parser.add_argument(
        "--output-dir",
        default="artifacts/asset-extract",
        help="Output directory for extracted data (default: artifacts/asset-extract)",
    )
    parser.add_argument(
        "--file-pattern",
        default=DEFAULT_FILE_PATTERN,
        help=f"Regex for input file names (default: {DEFAULT_FILE_PATTERN})",
    )
    parser.add_argument(
        "--types",
        default=",".join(DEFAULT_TYPES),
        help=(
            "Comma-separated Unity object types to include "
            "(default: AudioClip,AnimationClip,Sprite,Texture2D,TextAsset)"
        ),
    )
    parser.add_argument(
        "--name-pattern",
        default="",
        help="Optional regex applied to object name before export.",
    )
    parser.add_argument(
        "--max-per-type",
        type=int,
        default=0,
        help="Optional per-type export cap (0 = no limit).",
    )
    parser.add_argument(
        "--metadata-only",
        action="store_true",
        help="Do not write extracted asset files; write metadata only.",
    )
    return parser.parse_args()


def sanitize_name(value: str) -> str:
    normalized = value.strip() if value else ""
    if not normalized:
        return "unnamed"
    return SAFE_NAME_PATTERN.sub("_", normalized).strip("._") or "unnamed"


def ensure_dir(path: Path) -> None:
    path.mkdir(parents=True, exist_ok=True)


def get_candidate_files(data_dir: Path, file_pattern: re.Pattern[str]) -> list[Path]:
    files = [p for p in data_dir.iterdir() if p.is_file() and file_pattern.search(p.name)]
    return sorted(files, key=lambda p: p.name.lower())


def write_bytes(path: Path, payload: bytes) -> None:
    ensure_dir(path.parent)
    path.write_bytes(payload)


def write_json(path: Path, payload: Any) -> None:
    ensure_dir(path.parent)
    path.write_text(json.dumps(payload, ensure_ascii=False, indent=2), encoding="utf-8")


def write_text(path: Path, payload: str) -> None:
    ensure_dir(path.parent)
    path.write_text(payload, encoding="utf-8")


def extract_audio_clip(
    data: Any,
    asset_stem: str,
    target_dir: Path,
) -> list[str]:
    written: list[str] = []
    samples = getattr(data, "samples", None)
    if not samples:
        return written

    for sample_name, sample_data in samples.items():
        safe_sample = sanitize_name(sample_name or "sample")
        output_path = target_dir / f"{asset_stem}_{safe_sample}"
        if output_path.suffix == "":
            output_path = output_path.with_suffix(".wav")
        write_bytes(output_path, sample_data)
        written.append(str(output_path))
    return written


def extract_texture_or_sprite(data: Any, asset_stem: str, target_dir: Path) -> list[str]:
    written: list[str] = []
    image = getattr(data, "image", None)
    if image is None:
        return written

    output_path = target_dir / f"{asset_stem}.png"
    ensure_dir(output_path.parent)
    image.save(output_path)
    written.append(str(output_path))
    return written


def extract_animation_clip(data_obj: Any, asset_stem: str, target_dir: Path) -> list[str]:
    written: list[str] = []
    try:
        tree = data_obj.read_typetree()
    except Exception:
        return written

    output_path = target_dir / f"{asset_stem}.json"
    write_json(output_path, tree)
    written.append(str(output_path))
    return written


def extract_text_asset(data: Any, asset_stem: str, target_dir: Path) -> list[str]:
    written: list[str] = []
    script = getattr(data, "script", b"")
    output_path = target_dir / f"{asset_stem}.txt"

    if isinstance(script, str):
        write_text(output_path, script)
    else:
        raw = bytes(script or b"")
        if not raw:
            return written
        try:
            write_text(output_path, raw.decode("utf-8"))
        except UnicodeDecodeError:
            output_path = output_path.with_suffix(".bytes")
            write_bytes(output_path, raw)

    written.append(str(output_path))
    return written


def extract_mono_behaviour(
    data_obj: Any,
    asset_stem: str,
    target_dir: Path,
) -> tuple[list[str], dict[str, Any]]:
    written: list[str] = []
    info: dict[str, Any] = {}

    try:
        tree = data_obj.read_typetree()
    except Exception as exc:
        info["typetreeError"] = str(exc)
        return written, info

    if isinstance(tree, dict):
        info["name"] = tree.get("m_Name", "")
        script_ptr = tree.get("m_Script")
        if isinstance(script_ptr, dict):
            info["scriptPathId"] = script_ptr.get("m_PathID")
            info["scriptFileId"] = script_ptr.get("m_FileID")

    output_path = target_dir / f"{asset_stem}.json"
    write_json(output_path, tree)
    written.append(str(output_path))
    return written, info


def main() -> int:
    args = parse_args()
    data_dir = Path(args.data_dir)
    output_dir = Path(args.output_dir)
    ensure_dir(output_dir)

    if not data_dir.exists():
        print(f"ERROR: data directory not found: {data_dir}")
        return 1

    try:
        file_pattern = re.compile(args.file_pattern, re.IGNORECASE)
    except re.error as exc:
        print(f"ERROR: invalid --file-pattern regex: {exc}")
        return 1

    name_pattern = None
    if args.name_pattern:
        try:
            name_pattern = re.compile(args.name_pattern, re.IGNORECASE)
        except re.error as exc:
            print(f"ERROR: invalid --name-pattern regex: {exc}")
            return 1

    selected_types = {part.strip() for part in args.types.split(",") if part.strip()}
    if not selected_types:
        print("ERROR: no asset types selected.")
        return 1

    files = get_candidate_files(data_dir, file_pattern)
    if not files:
        print(f"ERROR: no files matched pattern under: {data_dir}")
        return 1

    metadata_path = output_dir / "assets.jsonl"
    summary_path = output_dir / "summary.json"
    errors_path = output_dir / "errors.log"

    type_counts: Counter[str] = Counter()
    file_counts: Counter[str] = Counter()
    exported_by_type: Counter[str] = Counter()
    error_lines: list[str] = []

    with metadata_path.open("w", encoding="utf-8") as out:
        for file_index, file_path in enumerate(files, 1):
            print(f"[{file_index}/{len(files)}] scanning {file_path.name}")

            try:
                env = UnityPy.load(str(file_path))
            except Exception as exc:
                message = f"{file_path.name}: load failed: {exc}"
                error_lines.append(message)
                print(f"  WARN {message}")
                continue

            for obj in env.objects:
                obj_type = obj.type.name
                if obj_type not in selected_types:
                    continue

                type_counts[obj_type] += 1
                file_counts[file_path.name] += 1

                record: dict[str, Any] = {
                    "sourceFile": file_path.name,
                    "type": obj_type,
                    "pathId": obj.path_id,
                    "name": "",
                    "exportedFiles": [],
                    "status": "indexed",
                }

                if args.max_per_type > 0 and exported_by_type[obj_type] >= args.max_per_type:
                    record["status"] = "skipped_max_per_type"
                    out.write(json.dumps(record, ensure_ascii=False) + "\n")
                    continue

                if obj_type == "MonoBehaviour":
                    raw_name = ""
                    if not args.metadata_only:
                        type_dir = output_dir / obj_type
                        safe_name = "unnamed"
                        asset_stem = f"{file_path.stem}_pid{obj.path_id}_{safe_name}"
                        try:
                            files_written, info = extract_mono_behaviour(obj, asset_stem, type_dir)
                            record["exportedFiles"] = files_written
                            if "name" in info:
                                raw_name = str(info["name"] or "")
                            if "scriptPathId" in info:
                                record["scriptPathId"] = info["scriptPathId"]
                            if "scriptFileId" in info:
                                record["scriptFileId"] = info["scriptFileId"]
                            if "typetreeError" in info:
                                record["typetreeError"] = info["typetreeError"]
                        except Exception as exc:
                            record["status"] = "export_error"
                            record["error"] = str(exc)
                            out.write(json.dumps(record, ensure_ascii=False) + "\n")
                            continue
                    else:
                        try:
                            tree = obj.read_typetree()
                            if isinstance(tree, dict):
                                raw_name = str(tree.get("m_Name", "") or "")
                        except Exception as exc:
                            record["status"] = "read_error"
                            record["error"] = str(exc)
                            out.write(json.dumps(record, ensure_ascii=False) + "\n")
                            continue

                    if name_pattern and not name_pattern.search(raw_name):
                        continue

                    record["name"] = raw_name
                else:
                    try:
                        data = obj.read()
                    except Exception as exc:
                        record["status"] = "read_error"
                        record["error"] = str(exc)
                        out.write(json.dumps(record, ensure_ascii=False) + "\n")
                        continue

                    raw_name = getattr(data, "name", "") or ""
                    if name_pattern and not name_pattern.search(raw_name):
                        continue

                    safe_name = sanitize_name(raw_name)
                    asset_stem = f"{file_path.stem}_pid{obj.path_id}_{safe_name}"
                    record["name"] = raw_name

                    if not args.metadata_only:
                        type_dir = output_dir / obj_type

                        try:
                            if obj_type == "AudioClip":
                                record["exportedFiles"] = extract_audio_clip(data, asset_stem, type_dir)
                            elif obj_type in {"Texture2D", "Sprite"}:
                                record["exportedFiles"] = extract_texture_or_sprite(data, asset_stem, type_dir)
                            elif obj_type == "AnimationClip":
                                record["exportedFiles"] = extract_animation_clip(obj, asset_stem, type_dir)
                            elif obj_type == "TextAsset":
                                record["exportedFiles"] = extract_text_asset(data, asset_stem, type_dir)
                        except Exception as exc:
                            record["status"] = "export_error"
                            record["error"] = str(exc)
                            out.write(json.dumps(record, ensure_ascii=False) + "\n")
                            continue

                if record["exportedFiles"]:
                    exported_by_type[obj_type] += 1
                    record["status"] = "exported"
                else:
                    record["status"] = "indexed_only"

                out.write(json.dumps(record, ensure_ascii=False) + "\n")

    summary = {
        "dataDir": str(data_dir),
        "outputDir": str(output_dir),
        "selectedTypes": sorted(selected_types),
        "matchedFiles": [p.name for p in files],
        "indexedCountByType": dict(sorted(type_counts.items())),
        "exportedCountByType": dict(sorted(exported_by_type.items())),
        "indexedCountBySourceFile": dict(sorted(file_counts.items())),
        "metadataPath": str(metadata_path),
        "errorsPath": str(errors_path),
    }
    write_json(summary_path, summary)

    if error_lines:
        write_text(errors_path, "\n".join(error_lines) + "\n")

    print("")
    print(f"Done. Metadata: {metadata_path}")
    print(f"Done. Summary:  {summary_path}")
    if error_lines:
        print(f"Done. Errors:   {errors_path} ({len(error_lines)} lines)")

    return 0


if __name__ == "__main__":
    sys.exit(main())
