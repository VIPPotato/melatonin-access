using System;
using System.Collections.Generic;

namespace MelatoninAccess
{
    /// <summary>
    /// Centralized localization for all mod-generated screen reader strings.
    /// Language follows the in-game language index from SaveManager.GetLang().
    /// </summary>
    public static class Loc
    {
        private const int LanguageCount = 10;
        private const int EnglishLang = 0;

        // Language index map (SaveManager.GetLang):
        // 0=en, 1=zh-Hans, 2=zh-Hant, 3=ja, 4=ko, 5=vi, 6=fr, 7=de, 8=es, 9=pt
        private static readonly Dictionary<string, string>[] _translations = new Dictionary<string, string>[LanguageCount];
        private static bool _initialized;
        private static int _currentLang = EnglishLang;

        /// <summary>
        /// Initializes translation dictionaries.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;

            for (int i = 0; i < LanguageCount; i++)
            {
                _translations[i] = new Dictionary<string, string>();
            }

            InitializeStrings();
            RefreshLanguage();
            _initialized = true;
        }

        /// <summary>
        /// Refreshes the current language from game settings.
        /// </summary>
        public static void RefreshLanguage()
        {
            _currentLang = NormalizeLanguageIndex(GetGameLanguageIndex());
        }

        /// <summary>
        /// Gets a localized string by key.
        /// </summary>
        public static string Get(string key)
        {
            if (!_initialized) Initialize();

            // Keep language in sync even if changed outside LangMenu.
            int latestLang = NormalizeLanguageIndex(GetGameLanguageIndex());
            if (latestLang != _currentLang)
            {
                _currentLang = latestLang;
            }

            if (_translations[_currentLang].TryGetValue(key, out string value))
            {
                return value;
            }

            if (_translations[EnglishLang].TryGetValue(key, out string english))
            {
                return english;
            }

            return key;
        }

        /// <summary>
        /// Gets a localized string by key and formats it with placeholders.
        /// </summary>
        public static string Get(string key, params object[] args)
        {
            string template = Get(key);
            try
            {
                return string.Format(template, args);
            }
            catch
            {
                return template;
            }
        }

        private static int GetGameLanguageIndex()
        {
            try
            {
                return SaveManager.GetLang();
            }
            catch
            {
                return EnglishLang;
            }
        }

        private static int NormalizeLanguageIndex(int lang)
        {
            return (lang >= 0 && lang < LanguageCount) ? lang : EnglishLang;
        }

        private static void Add(
            string key,
            string en,
            string zhHans,
            string zhHant,
            string ja,
            string ko,
            string vi,
            string fr,
            string de,
            string es,
            string pt)
        {
            _translations[0][key] = en;
            _translations[1][key] = zhHans;
            _translations[2][key] = zhHant;
            _translations[3][key] = ja;
            _translations[4][key] = ko;
            _translations[5][key] = vi;
            _translations[6][key] = fr;
            _translations[7][key] = de;
            _translations[8][key] = es;
            _translations[9][key] = pt;
        }

        private static void InitializeStrings()
        {
            Add(
                "mod_loaded",
                "Melatonin Access mod loaded.",
                "Melatonin Access 模组已加载。",
                "Melatonin Access 模組已載入。",
                "Melatonin Access mod loaded.",
                "Melatonin Access mod loaded.",
                "Mod Melatonin Access da duoc tai.",
                "Mod Melatonin Access charge.",
                "Melatonin Access Mod geladen.",
                "Mod Melatonin Access cargado.",
                "Mod Melatonin Access carregado.");

            Add(
                "mod_active_help",
                "Melatonin Access is active. Use arrow keys to navigate menus.",
                "Melatonin Access 已启用。使用方向键浏览菜单。",
                "Melatonin Access 已啟用。使用方向鍵瀏覽選單。",
                "Melatonin Access は有効です。方向キーでメニューを移動します。",
                "Melatonin Access가 활성화되었습니다. 방향키로 메뉴를 이동하세요.",
                "Melatonin Access dang hoat dong. Dung phim mui ten de dieu huong menu.",
                "Melatonin Access est actif. Utilisez les fleches pour naviguer dans les menus.",
                "Melatonin Access ist aktiv. Nutze die Pfeiltasten zur Menunavigation.",
                "Melatonin Access esta activo. Usa las flechas para navegar por los menus.",
                "Melatonin Access esta ativo. Use as setas para navegar nos menus.");

            Add(
                "intro_fallback",
                "Melatonin Access ready. Press {0} to start. Press Tab or Triangle/Y for language.",
                "Melatonin Access 已就绪。按 {0} 开始。按 Tab 或 Triangle/Y 选择语言。",
                "Melatonin Access 已就緒。按 {0} 開始。按 Tab 或 Triangle/Y 選擇語言。",
                "Melatonin Access の準備ができました。{0} で開始。Tab または Triangle/Y で言語を変更します。",
                "Melatonin Access 준비 완료. {0} 키로 시작하고 Tab 또는 Triangle/Y로 언어를 변경하세요.",
                "Melatonin Access san sang. Nhan {0} de bat dau. Nhan Tab hoac Triangle/Y de doi ngon ngu.",
                "Melatonin Access est pret. Appuyez sur {0} pour commencer. Appuyez sur Tab ou Triangle/Y pour la langue.",
                "Melatonin Access ist bereit. Drucke {0} zum Starten. Tab oder Triangle/Y fur Sprache.",
                "Melatonin Access listo. Pulsa {0} para comenzar. Pulsa Tab o Triangle/Y para idioma.",
                "Melatonin Access pronto. Pressione {0} para iniciar. Pressione Tab ou Triangle/Y para idioma.");

            Add(
                "language_selected",
                "Language selected.",
                "已选择语言。",
                "已選擇語言。",
                "言語を選択しました。",
                "언어를 선택했습니다.",
                "Da chon ngon ngu.",
                "Langue selectionnee.",
                "Sprache ausgewahlt.",
                "Idioma seleccionado.",
                "Idioma selecionado.");

            Add(
                "language_menu",
                "Language menu",
                "语言菜单",
                "語言選單",
                "言語メニュー",
                "언어 메뉴",
                "Menu ngon ngu",
                "Menu langue",
                "Sprachmenu",
                "Menu de idioma",
                "Menu de idioma");

            Add(
                "menu_suffix",
                "{0} menu",
                "{0} 菜单",
                "{0} 選單",
                "{0} メニュー",
                "{0} 메뉴",
                "Menu {0}",
                "Menu {0}",
                "{0} Menu",
                "Menu {0}",
                "Menu {0}");

            Add(
                "order_of",
                "{0} of {1}",
                "第 {0}/{1} 项",
                "第 {0}/{1} 項",
                "{0}/{1}",
                "{0}/{1}",
                "{0} tren {1}",
                "{0} sur {1}",
                "{0} von {1}",
                "{0} de {1}",
                "{0} de {1}");

            Add(
                "chapter_locked",
                "Chapter {0} locked",
                "第 {0} 章已锁定",
                "第 {0} 章已鎖定",
                "チャプター {0} はロックされています",
                "{0} 장이 잠겨 있습니다",
                "Chuong {0} da bi khoa",
                "Chapitre {0} verrouille",
                "Kapitel {0} gesperrt",
                "Capitulo {0} bloqueado",
                "Capitulo {0} bloqueado");

            Add(
                "option_with_slider",
                "{0} slider {1}",
                "{0} 滑块 {1}",
                "{0} 滑桿 {1}",
                "{0} スライダー {1}",
                "{0} 슬라이더 {1}",
                "{0} thanh truot {1}",
                "{0} curseur {1}",
                "{0} Regler {1}",
                "{0} deslizador {1}",
                "{0} controle deslizante {1}");

            Add(
                "slider_value",
                "Slider {0}",
                "滑块 {0}",
                "滑桿 {0}",
                "スライダー {0}",
                "슬라이더 {0}",
                "Thanh truot {0}",
                "Curseur {0}",
                "Regler {0}",
                "Deslizador {0}",
                "Controle deslizante {0}");

            Add(
                "state_on",
                "On",
                "开",
                "開",
                "オン",
                "켜짐",
                "Bat",
                "Active",
                "Ein",
                "Activado",
                "Ligado");

            Add(
                "state_off",
                "Off",
                "关",
                "關",
                "オフ",
                "꺼짐",
                "Tat",
                "Desactive",
                "Aus",
                "Desactivado",
                "Desligado");

            Add(
                "fullscreen",
                "Fullscreen",
                "全屏",
                "全螢幕",
                "フルスクリーン",
                "전체 화면",
                "Toan man hinh",
                "Plein ecran",
                "Vollbild",
                "Pantalla completa",
                "Tela cheia");

            Add(
                "windowed",
                "Windowed",
                "窗口",
                "視窗",
                "ウィンドウ",
                "창 모드",
                "Cua so",
                "Fenetre",
                "Fenstermodus",
                "Ventana",
                "Janela");

            Add(
                "calibration_tool_intro",
                "Calibration tool. Adjust offset to match the beat.",
                "校准工具。调整偏移以匹配节拍。",
                "校準工具。調整偏移以對齊節拍。",
                "キャリブレーションツール。オフセットを調整してビートに合わせます。",
                "보정 도구. 오프셋을 조정해 비트에 맞추세요.",
                "Cong cu hieu chinh. Dieu chinh do lech de khop nhip.",
                "Outil de calibration. Ajustez le decalage pour correspondre au rythme.",
                "Kalibrierungswerkzeug. Passe den Offset an den Beat an.",
                "Herramienta de calibracion. Ajusta el desfase para coincidir con el ritmo.",
                "Ferramenta de calibracao. Ajuste o deslocamento para combinar com a batida.");

            Add(
                "calibration_offset",
                "Offset {0}",
                "偏移 {0}",
                "偏移 {0}",
                "オフセット {0}",
                "오프셋 {0}",
                "Do lech {0}",
                "Decalage {0}",
                "Offset {0}",
                "Desfase {0}",
                "Deslocamento {0}");

            Add(
                "downloaded_levels_loading",
                "Downloaded Levels. Loading...",
                "已下载关卡。正在加载...",
                "已下載關卡。正在載入...",
                "ダウンロード済みレベル。読み込み中...",
                "다운로드된 레벨. 불러오는 중...",
                "Cap da tai xuong. Dang tai...",
                "Niveaux telecharges. Chargement...",
                "Heruntergeladene Level. Laden...",
                "Niveles descargados. Cargando...",
                "Niveis baixados. Carregando...");

            Add(
                "downloaded_levels_page_total",
                "{0} levels total. Page {1} of {2}",
                "共 {0} 个关卡。第 {1}/{2} 页",
                "共 {0} 個關卡。第 {1}/{2} 頁",
                "合計 {0} レベル。{1}/{2} ページ",
                "총 {0}개 레벨. {1}/{2} 페이지",
                "Tong cong {0} cap. Trang {1} tren {2}",
                "{0} niveaux au total. Page {1} sur {2}",
                "{0} Level insgesamt. Seite {1} von {2}",
                "{0} niveles en total. Pagina {1} de {2}",
                "{0} niveis no total. Pagina {1} de {2}");

            Add(
                "next_page",
                "Next page",
                "下一页",
                "下一頁",
                "次のページ",
                "다음 페이지",
                "Trang tiep theo",
                "Page suivante",
                "Nachte Seite",
                "Pagina siguiente",
                "Proxima pagina");

            Add(
                "previous_page",
                "Previous page",
                "上一页",
                "上一頁",
                "前のページ",
                "이전 페이지",
                "Trang truoc",
                "Page precedente",
                "Vorherige Seite",
                "Pagina anterior",
                "Pagina anterior");

            Add(
                "page_of",
                "Page {0} of {1}",
                "第 {0}/{1} 页",
                "第 {0}/{1} 頁",
                "{0}/{1} ページ",
                "{0}/{1} 페이지",
                "Trang {0} tren {1}",
                "Page {0} sur {1}",
                "Seite {0} von {1}",
                "Pagina {0} de {1}",
                "Pagina {0} de {1}");

            Add(
                "item_of",
                "{0}, item {1} of {2}",
                "{0}，第 {1}/{2} 项",
                "{0}，第 {1}/{2} 項",
                "{0}、{1}/{2}",
                "{0}, {1}/{2}",
                "{0}, muc {1} tren {2}",
                "{0}, element {1} sur {2}",
                "{0}, Eintrag {1} von {2}",
                "{0}, elemento {1} de {2}",
                "{0}, item {1} de {2}");

            Add(
                "achievements_menu",
                "Achievements menu",
                "成就菜单",
                "成就選單",
                "実績メニュー",
                "도전과제 메뉴",
                "Menu thanh tich",
                "Menu des succes",
                "Erfolge-Menue",
                "Menu de logros",
                "Menu de conquistas");

            Add(
                "locked_achievement",
                "Locked achievement. {0} of {1}",
                "未解锁成就。第 {0}/{1} 项",
                "未解鎖成就。第 {0}/{1} 項",
                "未解除の実績。{0}/{1}",
                "잠긴 도전과제. {0}/{1}",
                "Thanh tich bi khoa. {0} tren {1}",
                "Succes verrouille. {0} sur {1}",
                "Gesperrter Erfolg. {0} von {1}",
                "Logro bloqueado. {0} de {1}",
                "Conquista bloqueada. {0} de {1}");

            Add(
                "achievement_with_desc",
                "{0}: {1}. {2} of {3}",
                "{0}：{1}。第 {2}/{3} 项",
                "{0}：{1}。第 {2}/{3} 項",
                "{0}: {1}。{2}/{3}",
                "{0}: {1}. {2}/{3}",
                "{0}: {1}. {2} tren {3}",
                "{0} : {1}. {2} sur {3}",
                "{0}: {1}. {2} von {3}",
                "{0}: {1}. {2} de {3}",
                "{0}: {1}. {2} de {3}");

            Add(
                "arrived_at",
                "Arrived at {0}",
                "到达 {0}",
                "到達 {0}",
                "{0} に到着",
                "{0}에 도착",
                "Da den {0}",
                "Arrive a {0}",
                "Angekommen bei {0}",
                "Llegaste a {0}",
                "Chegou em {0}");

            Add(
                "mode_menu",
                "Mode menu",
                "模式菜单",
                "模式選單",
                "モードメニュー",
                "모드 메뉴",
                "Menu che do",
                "Menu de mode",
                "Modus-Menue",
                "Menu de modo",
                "Menu de modo");

            Add(
                "teleport_arrived_stars",
                "{0}. {1} stars.",
                "{0}。{1} 星。",
                "{0}。{1} 星。",
                "{0}。{1} スター。",
                "{0}. 별 {1}개.",
                "{0}. {1} sao.",
                "{0}. {1} etoiles.",
                "{0}. {1} Sterne.",
                "{0}. {1} estrellas.",
                "{0}. {1} estrelas.");

            Add(
                "unknown_level",
                "Unknown level",
                "未知关卡",
                "未知關卡",
                "不明なレベル",
                "알 수 없는 레벨",
                "Cap do khong ro",
                "Niveau inconnu",
                "Unbekanntes Level",
                "Nivel desconocido",
                "Nivel desconhecido");

            Add(
                "editor_cursor_empty",
                "Phrase {0}, Bar {1}, Beat {2}. Empty.",
                "乐句 {0}，小节 {1}，节拍 {2}。空。",
                "樂句 {0}，小節 {1}，節拍 {2}。空白。",
                "フレーズ {0}、小節 {1}、拍 {2}。空です。",
                "프레이즈 {0}, 마디 {1}, 비트 {2}. 비어 있음.",
                "Cum {0}, o nhip {1}, phach {2}. Trong.",
                "Phrase {0}, mesure {1}, temps {2}. Vide.",
                "Phrase {0}, Takt {1}, Schlag {2}. Leer.",
                "Frase {0}, compas {1}, tiempo {2}. Vacio.",
                "Frase {0}, compasso {1}, batida {2}. Vazio.");

            Add(
                "editor_cursor_content",
                "Phrase {0}, Bar {1}, Beat {2}. {3}.",
                "乐句 {0}，小节 {1}，节拍 {2}。{3}。",
                "樂句 {0}，小節 {1}，節拍 {2}。{3}。",
                "フレーズ {0}、小節 {1}、拍 {2}。{3}。",
                "프레이즈 {0}, 마디 {1}, 비트 {2}. {3}.",
                "Cum {0}, o nhip {1}, phach {2}. {3}.",
                "Phrase {0}, mesure {1}, temps {2}. {3}.",
                "Phrase {0}, Takt {1}, Schlag {2}. {3}.",
                "Frase {0}, compas {1}, tiempo {2}. {3}.",
                "Frase {0}, compasso {1}, batida {2}. {3}.");

            Add(
                "editor_placed",
                "Placed {0}",
                "已放置 {0}",
                "已放置 {0}",
                "{0} を配置しました",
                "{0} 배치됨",
                "Da dat {0}",
                "{0} place",
                "{0} platziert",
                "Colocado {0}",
                "Colocado {0}");

            Add(
                "editor_removed",
                "Removed",
                "已移除",
                "已移除",
                "削除しました",
                "삭제됨",
                "Da xoa",
                "Supprime",
                "Entfernt",
                "Eliminado",
                "Removido");

            Add(
                "editor_tool_select",
                "Tool select",
                "工具选择",
                "工具選擇",
                "ツール選択",
                "도구 선택",
                "Chon cong cu",
                "Selection d'outil",
                "Werkzeugauswahl",
                "Seleccion de herramienta",
                "Selecao de ferramenta");

            Add(
                "editor_ready",
                "Level editor ready",
                "关卡编辑器已就绪",
                "關卡編輯器已就緒",
                "レベルエディター準備完了",
                "레벨 에디터 준비 완료",
                "Trinh chinh sua cap da san sang",
                "Editeur de niveau pret",
                "Level-Editor bereit",
                "Editor de niveles listo",
                "Editor de nivel pronto");

            Add(
                "editor_page",
                "Page {0}",
                "页面 {0}",
                "頁面 {0}",
                "ページ {0}",
                "페이지 {0}",
                "Trang {0}",
                "Page {0}",
                "Seite {0}",
                "Pagina {0}",
                "Pagina {0}");

            Add(
                "cue_space",
                "Space",
                "空格",
                "空白鍵",
                "スペース",
                "스페이스",
                "Space",
                "Espace",
                "Leertaste",
                "Espacio",
                "Espaco");

            Add(
                "cue_left",
                "Left",
                "左",
                "左",
                "左",
                "왼쪽",
                "Trai",
                "Gauche",
                "Links",
                "Izquierda",
                "Esquerda");

            Add(
                "cue_right",
                "Right",
                "右",
                "右",
                "右",
                "오른쪽",
                "Phai",
                "Droite",
                "Rechts",
                "Derecha",
                "Direita");

            Add(
                "cue_both",
                "Both",
                "双键",
                "雙鍵",
                "両方",
                "양쪽",
                "Ca hai",
                "Les deux",
                "Beide",
                "Ambos",
                "Ambos");

            Add(
                "cue_hold",
                "Hold",
                "按住",
                "按住",
                "ホールド",
                "홀드",
                "Giu",
                "Maintenir",
                "Halten",
                "Mantener",
                "Segurar");

            Add(
                "results_stats",
                "Perfect: {0}, Late: {1}, Early: {2}, Miss: {3}.",
                "完美：{0}，偏晚：{1}，偏早：{2}，失误：{3}。",
                "完美：{0}，偏晚：{1}，偏早：{2}，失誤：{3}。",
                "Perfect: {0}, Late: {1}, Early: {2}, Miss: {3}。",
                "Perfect: {0}, Late: {1}, Early: {2}, Miss: {3}.",
                "Perfect: {0}, Late: {1}, Early: {2}, Miss: {3}.",
                "Parfait : {0}, Tard : {1}, Tôt : {2}, Rate : {3}.",
                "Perfekt: {0}, Spaet: {1}, Frueh: {2}, Verpasst: {3}.",
                "Perfecto: {0}, Tarde: {1}, Temprano: {2}, Fallo: {3}.",
                "Perfeito: {0}, Tarde: {1}, Cedo: {2}, Erro: {3}.");

            Add(
                "stage_end_position",
                "{0}, {1} of {2}",
                "{0}，第 {1}/{2} 项",
                "{0}，第 {1}/{2} 項",
                "{0}、{1}/{2}",
                "{0}, {1}/{2}",
                "{0}, {1} tren {2}",
                "{0}, {1} sur {2}",
                "{0}, {1} von {2}",
                "{0}, {1} de {2}",
                "{0}, {1} de {2}");
        }
    }
}
