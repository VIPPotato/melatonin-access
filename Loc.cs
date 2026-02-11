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

        /// <summary>
        /// Gets a localized level/dream name from the internal dream key.
        /// </summary>
        public static string GetDreamName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName)) return Get("unknown_level");

            string normalized = rawName.Trim().ToLowerInvariant();
            string key = "dream_name_" + normalized;
            string localized = Get(key);
            if (localized != key) return localized;

            if (normalized.Length == 1) return normalized.ToUpperInvariant();
            return char.ToUpperInvariant(normalized[0]) + normalized.Substring(1);
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
                "Melatonin Access mod を読み込みました。",
                "Melatonin Access 모드가 로드되었습니다.",
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
                "debug_enabled",
                "Debug mode enabled.",
                "调试模式已启用。",
                "偵錯模式已啟用。",
                "デバッグモードを有効にしました。",
                "디버그 모드가 켜졌습니다.",
                "Da bat che do giai loi.",
                "Mode debug active.",
                "Debug-Modus aktiviert.",
                "Modo depuracion activado.",
                "Modo de depuracao ativado.");

            Add(
                "debug_disabled",
                "Debug mode disabled.",
                "调试模式已禁用。",
                "偵錯模式已停用。",
                "デバッグモードを無効にしました。",
                "디버그 모드가 꺼졌습니다.",
                "Da tat che do giai loi.",
                "Mode debug desactive.",
                "Debug-Modus deaktiviert.",
                "Modo depuracion desactivado.",
                "Modo de depuracao desativado.");

            Add(
                "rhythm_cues_enabled",
                "Rhythm cue announcements enabled.",
                "节奏提示播报已启用。",
                "節奏提示播報已啟用。",
                "リズムキューの読み上げを有効にしました。",
                "리듬 큐 안내를 켰습니다.",
                "Da bat thong bao nhac nhip.",
                "Annonces des reperes rythmiques activees.",
                "Rhythmus-Hinweise aktiviert.",
                "Anuncios de ritmos activados.",
                "Anuncios de ritmo ativados.");

            Add(
                "rhythm_cues_disabled",
                "Rhythm cue announcements disabled.",
                "节奏提示播报已禁用。",
                "節奏提示播報已停用。",
                "リズムキューの読み上げを無効にしました。",
                "리듬 큐 안내를 껐습니다.",
                "Da tat thong bao nhac nhip.",
                "Annonces des reperes rythmiques desactivees.",
                "Rhythmus-Hinweise deaktiviert.",
                "Anuncios de ritmos desactivados.",
                "Anuncios de ritmo desativados.");

            Add(
                "menu_positions_enabled",
                "Menu position announcements enabled.",
                "菜单位置播报已启用。",
                "選單位置播報已啟用。",
                "メニュー位置の読み上げを有効にしました。",
                "메뉴 위치 안내를 켰습니다.",
                "Da bat thong bao vi tri menu.",
                "Annonces de position de menu activees.",
                "Menuepositionsansagen aktiviert.",
                "Anuncios de posicion de menu activados.",
                "Anuncios de posicao de menu ativados.");

            Add(
                "menu_positions_disabled",
                "Menu position announcements disabled.",
                "菜单位置播报已禁用。",
                "選單位置播報已停用。",
                "メニュー位置の読み上げを無効にしました。",
                "메뉴 위치 안내를 껐습니다.",
                "Da tat thong bao vi tri menu.",
                "Annonces de position de menu desactivees.",
                "Menuepositionsansagen deaktiviert.",
                "Anuncios de posicion de menu desactivados.",
                "Anuncios de posicao de menu desativados.");

            Add(
                "intro_welcome",
                "Press {0} to begin. Press {1} to change language.",
                "按 {0} 开始。按 {1} 更改语言。",
                "按 {0} 開始。按 {1} 更改語言。",
                "{0} で開始。{1} で言語を変更します。",
                "{0} 키로 시작하고 {1} 키로 언어를 변경하세요.",
                "Nhan {0} de bat dau. Nhan {1} de doi ngon ngu.",
                "Appuyez sur {0} pour commencer. Appuyez sur {1} pour changer la langue.",
                "Drucke {0} zum Starten. Drucke {1}, um die Sprache zu aendern.",
                "Pulsa {0} para comenzar. Pulsa {1} para cambiar el idioma.",
                "Pressione {0} para iniciar. Pressione {1} para alterar o idioma.");

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
                "credits_title",
                "Credits",
                "制作人员",
                "製作人員",
                "クレジット",
                "크레딧",
                "Danh de",
                "Generique",
                "Credits",
                "Creditos",
                "Creditos");

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
                "action_key_current_binding",
                "{0}, currently bound to {1}.",
                "{0}，当前绑定为 {1}。",
                "{0}，目前綁定為 {1}。",
                "{0}。現在の割り当ては {1} です。",
                "{0}, 현재 {1}에 바인딩됨.",
                "{0}, hien dang gan voi {1}.",
                "{0}, actuellement assigne a {1}.",
                "{0}, derzeit auf {1} gebunden.",
                "{0}, actualmente asignado a {1}.",
                "{0}, atualmente vinculado a {1}.");

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
                "calibration_timing_on_time",
                "On time.",
                "时机准确。",
                "時機準確。",
                "タイミングは正確です。",
                "타이밍이 정확합니다.",
                "Dung nhip.",
                "Au bon moment.",
                "Im Takt.",
                "A tiempo.",
                "No tempo.");

            Add(
                "calibration_timing_early_ms",
                "Early by {0} ms.",
                "提前了 {0} 毫秒。",
                "提前了 {0} 毫秒。",
                "{0} ミリ秒早いです。",
                "{0}ms 빨랐습니다.",
                "Som {0} ms.",
                "En avance de {0} ms.",
                "{0} ms zu frueh.",
                "Temprano por {0} ms.",
                "Adiantado em {0} ms.");

            Add(
                "calibration_timing_late_ms",
                "Late by {0} ms.",
                "慢了 {0} 毫秒。",
                "慢了 {0} 毫秒。",
                "{0} ミリ秒遅いです。",
                "{0}ms 늦었습니다.",
                "Tre {0} ms.",
                "En retard de {0} ms.",
                "{0} ms zu spaet.",
                "Tarde por {0} ms.",
                "Atrasado em {0} ms.");

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
                "downloaded_levels_total",
                "{0} levels total.",
                "共 {0} 个关卡。",
                "共 {0} 個關卡。",
                "合計 {0} レベル。",
                "총 {0}개 레벨.",
                "Tong cong {0} cap.",
                "{0} niveaux au total.",
                "{0} Level insgesamt.",
                "{0} niveles en total.",
                "{0} niveis no total.");

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
                "locked_achievement_plain",
                "Locked achievement.",
                "未解锁成就。",
                "未解鎖成就。",
                "未解除の実績。",
                "잠긴 도전과제.",
                "Thanh tich bi khoa.",
                "Succes verrouille.",
                "Gesperrter Erfolg.",
                "Logro bloqueado.",
                "Conquista bloqueada.");

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
                "achievement_with_desc_plain",
                "{0}: {1}.",
                "{0}：{1}。",
                "{0}：{1}。",
                "{0}: {1}。",
                "{0}: {1}.",
                "{0}: {1}.",
                "{0} : {1}.",
                "{0}: {1}.",
                "{0}: {1}.",
                "{0}: {1}.");

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
                "dream_about_level",
                "Dream about {0}",
                "关于 {0} 的梦境",
                "關於 {0} 的夢境",
                "{0} についての夢",
                "{0}에 대한 꿈",
                "Giac mo ve {0}",
                "Reve de {0}",
                "Traum ueber {0}",
                "Sueno sobre {0}",
                "Sonho sobre {0}");

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
                "teleport_arrived_one_star",
                "{0}. {1} star.",
                "{0}。{1} 星。",
                "{0}。{1} 星。",
                "{0}。{1} スター。",
                "{0}. 별 {1}개.",
                "{0}. {1} sao.",
                "{0}. {1} etoile.",
                "{0}. {1} Stern.",
                "{0}. {1} estrella.",
                "{0}. {1} estrela.");

            Add(
                "map_progress_status",
                "Map stars: {0}. Need {1} more to pass.",
                "地图星星：{0}。还需要 {1} 颗才能通过。",
                "地圖星星：{0}。還需要 {1} 顆才能通過。",
                "マップのスター: {0}。クリアまであと {1}。",
                "맵 별: {0}. 통과까지 {1}개 더 필요합니다.",
                "So sao ban do: {0}. Can them {1} sao de vuot qua.",
                "Etoiles de la carte : {0}. Il en faut encore {1} pour valider.",
                "Kartensterne: {0}. Noch {1} zum Bestehen noetig.",
                "Estrellas del mapa: {0}. Faltan {1} para completar.",
                "Estrelas do mapa: {0}. Faltam {1} para passar.");

            Add(
                "teleport_conflict_hint",
                "Bracket teleport is disabled because Action is bound to brackets. Use F9 and F10 for teleport.",
                "由于操作键绑定到方括号，方括号传送已禁用。请使用 F9 和 F10 传送。",
                "由於操作鍵綁定到中括號，中括號傳送已停用。請使用 F9 與 F10 傳送。",
                "アクションキーが角かっこに割り当てられているため、角かっこテレポートは無効です。F9 と F10 を使ってください。",
                "액션 키가 대괄호로 바인딩되어 대괄호 순간이동이 비활성화되었습니다. 순간이동은 F9/F10을 사용하세요.",
                "Dich chuyen bang ngoac vuong da bi tat vi phim Hanh dong dang gan vao ngoac vuong. Hay dung F9 va F10 de dich chuyen.",
                "La teleportation avec crochets est desactivee car l'action est assignee aux crochets. Utilisez F9 et F10.",
                "Klammer-Teleport ist deaktiviert, weil Aktion auf Klammern gebunden ist. Nutze F9 und F10.",
                "La teletransportacion con corchetes esta desactivada porque Accion usa corchetes. Usa F9 y F10.",
                "O teleporte com colchetes foi desativado porque a Acao esta vinculada a colchetes. Use F9 e F10.");

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
                "dream_name_career",
                "Career",
                "事业",
                "事業",
                "キャリア",
                "커리어",
                "Su nghiep",
                "Carriere",
                "Karriere",
                "Carrera",
                "Carreira");

            Add(
                "dream_name_dating",
                "Dating",
                "约会",
                "約會",
                "デート",
                "데이트",
                "Hen ho",
                "Rendez-vous",
                "Dating",
                "Citas",
                "Encontros");

            Add(
                "dream_name_desires",
                "Desires",
                "欲望",
                "慾望",
                "欲望",
                "욕망",
                "Ham muon",
                "Desirs",
                "Wuensche",
                "Deseos",
                "Desejos");

            Add(
                "dream_name_exercise",
                "Exercise",
                "运动",
                "運動",
                "エクササイズ",
                "운동",
                "Tap luyen",
                "Exercice",
                "Training",
                "Ejercicio",
                "Exercicio");

            Add(
                "dream_name_final",
                "Final",
                "最终",
                "最終",
                "ファイナル",
                "파이널",
                "Cuoi cung",
                "Final",
                "Finale",
                "Final",
                "Final");

            Add(
                "dream_name_followers",
                "Followers",
                "关注者",
                "關注者",
                "フォロワー",
                "팔로워",
                "Nguoi theo doi",
                "Abonnes",
                "Follower",
                "Seguidores",
                "Seguidores");

            Add(
                "dream_name_food",
                "Food",
                "食物",
                "食物",
                "フード",
                "음식",
                "Do an",
                "Nourriture",
                "Essen",
                "Comida",
                "Comida");

            Add(
                "dream_name_future",
                "Future",
                "未来",
                "未來",
                "未来",
                "미래",
                "Tuong lai",
                "Futur",
                "Zukunft",
                "Futuro",
                "Futuro");

            Add(
                "dream_name_indulgence",
                "Indulgence",
                "放纵",
                "放縱",
                "放縦",
                "방종",
                "Huong lac",
                "Indulgence",
                "Genuss",
                "Indulgencia",
                "Indulgencia");

            Add(
                "dream_name_meditation",
                "Meditation",
                "冥想",
                "冥想",
                "瞑想",
                "명상",
                "Thien",
                "Meditation",
                "Meditation",
                "Meditacion",
                "Meditacao");

            Add(
                "dream_name_mind",
                "Mind",
                "心灵",
                "心靈",
                "マインド",
                "마음",
                "Tam tri",
                "Esprit",
                "Geist",
                "Mente",
                "Mente");

            Add(
                "dream_name_money",
                "Money",
                "金钱",
                "金錢",
                "お金",
                "돈",
                "Tien bac",
                "Argent",
                "Geld",
                "Dinero",
                "Dinheiro");

            Add(
                "dream_name_nature",
                "Nature",
                "自然",
                "自然",
                "自然",
                "자연",
                "Thien nhien",
                "Nature",
                "Natur",
                "Naturaleza",
                "Natureza");

            Add(
                "dream_name_past",
                "Past",
                "过去",
                "過去",
                "過去",
                "과거",
                "Qua khu",
                "Passe",
                "Vergangenheit",
                "Pasado",
                "Passado");

            Add(
                "dream_name_pressure",
                "Pressure",
                "压力",
                "壓力",
                "プレッシャー",
                "압박",
                "Ap luc",
                "Pression",
                "Druck",
                "Presion",
                "Pressao");

            Add(
                "dream_name_setbacks",
                "Setbacks",
                "挫折",
                "挫折",
                "挫折",
                "좌절",
                "Tro ngai",
                "Revers",
                "Rueckschlaege",
                "Reveses",
                "Contratempos");

            Add(
                "dream_name_shopping",
                "Shopping",
                "购物",
                "購物",
                "ショッピング",
                "쇼핑",
                "Mua sam",
                "Shopping",
                "Einkaufen",
                "Compras",
                "Compras");

            Add(
                "dream_name_space",
                "Space",
                "太空",
                "太空",
                "宇宙",
                "우주",
                "Khong gian",
                "Espace",
                "Weltraum",
                "Espacio",
                "Espaco");

            Add(
                "dream_name_stress",
                "Stress",
                "压力",
                "壓力",
                "ストレス",
                "스트레스",
                "Cang thang",
                "Stress",
                "Stress",
                "Estres",
                "Estresse");

            Add(
                "dream_name_tech",
                "Tech",
                "科技",
                "科技",
                "テック",
                "테크",
                "Cong nghe",
                "Technologie",
                "Technik",
                "Tecnologia",
                "Tecnologia");

            Add(
                "dream_name_time",
                "Time",
                "时间",
                "時間",
                "時間",
                "시간",
                "Thoi gian",
                "Temps",
                "Zeit",
                "Tiempo",
                "Tempo");

            Add(
                "dream_name_tutorial",
                "Tutorial",
                "教程",
                "教學",
                "チュートリアル",
                "튜토리얼",
                "Huong dan",
                "Tutoriel",
                "Tutorial",
                "Tutorial",
                "Tutorial");

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
                "advanced_menu",
                "Advanced menu",
                "高级菜单",
                "進階選單",
                "高度なメニュー",
                "고급 메뉴",
                "Menu nang cao",
                "Menu avance",
                "Erweitertes Menue",
                "Menu avanzado",
                "Menu avancado");

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
                "tutorial_skip_prompt",
                "Tutorial. Press {0} to skip.",
                "教程。按 {0} 可跳过。",
                "教學。按 {0} 可跳過。",
                "チュートリアル。{0} でスキップします。",
                "튜토리얼. {0} 키로 건너뛸 수 있습니다.",
                "Huong dan. Nhan {0} de bo qua.",
                "Tutoriel. Appuyez sur {0} pour passer.",
                "Tutorial. Drucke {0}, um zu ueberspringen.",
                "Tutorial. Pulsa {0} para omitir.",
                "Tutorial. Pressione {0} para pular.");

            Add(
                "tutorial_label",
                "Tutorial.",
                "教程。",
                "教學。",
                "チュートリアル。",
                "튜토리얼.",
                "Huong dan.",
                "Tutoriel.",
                "Tutorial.",
                "Tutorial.",
                "Tutorial.");

            Add(
                "cue_space",
                "Space",
                "空格",
                "空白鍵",
                "スペース",
                "스페이스",
                "Phim cach",
                "Espace",
                "Leertaste",
                "Espacio",
                "Espaco");

            Add(
                "cue_press_action",
                "Press {0}",
                "按 {0}",
                "按 {0}",
                "{0} を押す",
                "{0} 누르기",
                "Nhan {0}",
                "Appuyer sur {0}",
                "{0} druecken",
                "Pulsa {0}",
                "Pressione {0}");

            Add(
                "cue_press_action_twice",
                "Press {0} twice",
                "按 {0} 两次",
                "按 {0} 兩次",
                "{0} を2回押す",
                "{0} 두 번 누르기",
                "Nhan {0} hai lan",
                "Appuyer deux fois sur {0}",
                "{0} zweimal druecken",
                "Pulsa {0} dos veces",
                "Pressione {0} duas vezes");

            Add(
                "cue_followers_rhythm_stop",
                "Press {0} to the rhythm. Stop on the audio cue.",
                "跟着节奏按 {0}。听到音频提示后停止。",
                "跟著節奏按 {0}。聽到音效提示後停止。",
                "リズムに合わせて {0} を押します。音の合図で止めます。",
                "리듬에 맞춰 {0}를 누르세요. 오디오 신호에서 멈추세요.",
                "Nhan {0} theo nhip. Dung lai khi nghe tin hieu am thanh.",
                "Appuyez sur {0} en rythme. Arretez au signal audio.",
                "Druecke {0} im Rhythmus. Beim Audio-Signal stoppen.",
                "Pulsa {0} al ritmo. Detente con la senal de audio.",
                "Pressione {0} no ritmo. Pare no sinal de audio.");

            Add(
                "cue_food_press_third_beat",
                "Press {0} on the third beat.",
                "在第3拍按 {0}。",
                "在第3拍按 {0}。",
                "3拍目で {0} を押す。",
                "3번째 박자에 {0}를 누르세요.",
                "Nhan {0} o nhip thu 3.",
                "Appuyez sur {0} au troisieme temps.",
                "Druecke {0} beim dritten Schlag.",
                "Pulsa {0} en el tercer pulso.",
                "Pressione {0} no terceiro tempo.");

            Add(
                "cue_food_press_fifth_beat",
                "Press {0} on the fifth beat.",
                "在第5拍按 {0}。",
                "在第5拍按 {0}。",
                "5拍目で {0} を押す。",
                "5번째 박자에 {0}를 누르세요.",
                "Nhan {0} o nhip thu 5.",
                "Appuyez sur {0} au cinquieme temps.",
                "Druecke {0} beim fuenften Schlag.",
                "Pulsa {0} en el quinto pulso.",
                "Pressione {0} no quinto tempo.");

            Add(
                "cue_food_press_fourth_beat",
                "Press {0} on the fourth beat.",
                "在第4拍按 {0}。",
                "在第4拍按 {0}。",
                "4拍目で {0} を押す。",
                "4번째 박자에 {0}를 누르세요.",
                "Nhan {0} o nhip thu 4.",
                "Appuyez sur {0} au quatrieme temps.",
                "Druecke {0} beim vierten Schlag.",
                "Pulsa {0} en el cuarto pulso.",
                "Pressione {0} no quarto tempo.");

            Add(
                "cue_shopping_repeat_patterns",
                "Repeat the audio cue patterns with {0}.",
                "用 {0} 重复音频提示节奏。",
                "用 {0} 重複音訊提示節奏。",
                "{0} で音のパターンを繰り返します。",
                "{0}로 오디오 큐 패턴을 반복하세요.",
                "Lap lai cac mau tin hieu am thanh bang {0}.",
                "Repetez les motifs audio avec {0}.",
                "Wiederhole die Audio-Muster mit {0}.",
                "Repite los patrones de audio con {0}.",
                "Repita os padroes de audio com {0}.");

            Add(
                "cue_hold_action",
                "Hold {0}",
                "按住 {0}",
                "按住 {0}",
                "{0} を長押し",
                "{0} 길게 누르기",
                "Giu {0}",
                "Maintenir {0}",
                "{0} halten",
                "Mantener {0}",
                "Segurar {0}");

            Add(
                "key_enter",
                "Enter",
                "回车",
                "Enter",
                "エンター",
                "엔터",
                "Enter",
                "Entree",
                "Eingabetaste",
                "Enter",
                "Enter");

            Add(
                "key_period",
                "Period",
                "句点",
                "句點",
                "ピリオド",
                "마침표",
                "Dau cham",
                "Point",
                "Punkt",
                "Punto",
                "Ponto");

            Add(
                "key_slash",
                "Slash",
                "斜杠",
                "斜線",
                "スラッシュ",
                "슬래시",
                "Dau gach cheo",
                "Barre oblique",
                "Schraegstrich",
                "Barra",
                "Barra");

            Add(
                "key_tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab",
                "Tab");

            Add(
                "key_escape",
                "Escape",
                "Esc",
                "Esc",
                "Esc",
                "Esc",
                "Esc",
                "Echap",
                "Esc",
                "Esc",
                "Esc");

            Add(
                "key_start",
                "Start",
                "开始键",
                "開始鍵",
                "スタート",
                "시작 버튼",
                "Start",
                "Start",
                "Start",
                "Start",
                "Start");

            Add(
                "help_title_screen",
                "Title screen help. Press {0} to begin. Press {1} to change language.",
                "标题界面帮助。按 {0} 开始。按 {1} 更改语言。",
                "標題畫面說明。按 {0} 開始。按 {1} 更改語言。",
                "タイトル画面ヘルプ。{0} で開始。{1} で言語変更。",
                "타이틀 화면 도움말. {0} 키로 시작하고 {1} 키로 언어를 변경합니다.",
                "Tro giup man hinh tieu de. Nhan {0} de bat dau. Nhan {1} de doi ngon ngu.",
                "Aide ecran titre. Appuyez sur {0} pour commencer. Appuyez sur {1} pour changer la langue.",
                "Titelbildschirm-Hilfe. Druecke {0} zum Starten. Druecke {1} fuer die Sprache.",
                "Ayuda de pantalla de titulo. Pulsa {0} para empezar. Pulsa {1} para cambiar idioma.",
                "Ajuda da tela inicial. Pressione {0} para comecar. Pressione {1} para mudar o idioma.");

            Add(
                "help_menu",
                "Menu help. Use Up and Down to move. Press {0} to select. Press {1} to go back.",
                "菜单帮助。使用上下方向移动。按 {0} 选择。按 {1} 返回。",
                "選單說明。使用上下方向移動。按 {0} 選擇。按 {1} 返回。",
                "メニューヘルプ。上下で移動。{0} で決定。{1} で戻る。",
                "메뉴 도움말. 위아래로 이동하고 {0} 키로 선택합니다. {1} 키로 돌아갑니다.",
                "Tro giup menu. Dung Len va Xuong de di chuyen. Nhan {0} de chon. Nhan {1} de quay lai.",
                "Aide menu. Utilisez Haut et Bas pour bouger. Appuyez sur {0} pour choisir. Appuyez sur {1} pour retour.",
                "Menue-Hilfe. Mit Hoch und Runter bewegen. Druecke {0} zum Waehlen. Druecke {1} zum Zurueckgehen.",
                "Ayuda de menu. Usa Arriba y Abajo para moverte. Pulsa {0} para seleccionar. Pulsa {1} para volver.",
                "Ajuda de menu. Use Cima e Baixo para mover. Pressione {0} para selecionar. Pressione {1} para voltar.");

            Add(
                "help_map",
                "Map help. Use [ and ] or F9 and F10 to move between levels. Press {0} to open the selected level. Press F1 for map stars.",
                "地图帮助。使用 [ 和 ] 或 F9 和 F10 在关卡间移动。按 {0} 打开所选关卡。按 F1 播报地图星星。",
                "地圖說明。使用 [ 和 ] 或 F9 與 F10 在關卡間移動。按 {0} 開啟所選關卡。按 F1 播報地圖星星。",
                "マップヘルプ。[ と ] または F9 と F10 でレベル間を移動。{0} で選択レベルを開く。F1 でマップスター。",
                "맵 도움말. [ 와 ] 또는 F9/F10으로 레벨 사이를 이동합니다. {0} 키로 선택한 레벨을 엽니다. F1으로 맵 별을 확인합니다.",
                "Tro giup ban do. Dung [ va ] hoac F9 va F10 de di giua cac man. Nhan {0} de mo man da chon. Nhan F1 de nghe sao ban do.",
                "Aide carte. Utilisez [ et ] ou F9 et F10 pour changer de niveau. Appuyez sur {0} pour ouvrir le niveau choisi. Appuyez sur F1 pour les etoiles de la carte.",
                "Karten-Hilfe. Nutze [ und ] oder F9 und F10 zum Wechseln zwischen Levels. Druecke {0}, um das ausgewaehlte Level zu oeffnen. Druecke F1 fuer Kartensterne.",
                "Ayuda del mapa. Usa [ y ] o F9 y F10 para moverte entre niveles. Pulsa {0} para abrir el nivel seleccionado. Pulsa F1 para las estrellas del mapa.",
                "Ajuda do mapa. Use [ e ] ou F9 e F10 para mover entre niveis. Pressione {0} para abrir o nivel selecionado. Pressione F1 para as estrelas do mapa.");

            Add(
                "help_mode_menu",
                "Mode menu help. Use Up and Down to choose mode. Press {0} to start. Press {1} to close this menu.",
                "模式菜单帮助。使用上下方向选择模式。按 {0} 开始。按 {1} 关闭此菜单。",
                "模式選單說明。使用上下方向選擇模式。按 {0} 開始。按 {1} 關閉此選單。",
                "モードメニューヘルプ。上下でモード選択。{0} で開始。{1} でこのメニューを閉じる。",
                "모드 메뉴 도움말. 위아래로 모드를 선택합니다. {0} 키로 시작하고 {1} 키로 이 메뉴를 닫습니다.",
                "Tro giup menu che do. Dung Len va Xuong de chon che do. Nhan {0} de bat dau. Nhan {1} de dong menu nay.",
                "Aide menu de mode. Utilisez Haut et Bas pour choisir un mode. Appuyez sur {0} pour lancer. Appuyez sur {1} pour fermer ce menu.",
                "Modusmenue-Hilfe. Mit Hoch und Runter Modus waehlen. Druecke {0} zum Starten. Druecke {1}, um dieses Menue zu schliessen.",
                "Ayuda del menu de modo. Usa Arriba y Abajo para elegir modo. Pulsa {0} para iniciar. Pulsa {1} para cerrar este menu.",
                "Ajuda do menu de modo. Use Cima e Baixo para escolher o modo. Pressione {0} para iniciar. Pressione {1} para fechar este menu.");

            Add(
                "help_results",
                "Results help. Use Up and Down to choose an option. Press {0} to confirm. Press {1} to go back.",
                "结果界面帮助。使用上下方向选择选项。按 {0} 确认。按 {1} 返回。",
                "結果畫面說明。使用上下方向選擇選項。按 {0} 確認。按 {1} 返回。",
                "リザルトヘルプ。上下で項目選択。{0} で決定。{1} で戻る。",
                "결과 화면 도움말. 위아래로 옵션을 선택합니다. {0} 키로 확인하고 {1} 키로 돌아갑니다.",
                "Tro giup ket qua. Dung Len va Xuong de chon tuy chon. Nhan {0} de xac nhan. Nhan {1} de quay lai.",
                "Aide resultats. Utilisez Haut et Bas pour choisir une option. Appuyez sur {0} pour confirmer. Appuyez sur {1} pour retour.",
                "Ergebnis-Hilfe. Mit Hoch und Runter Option waehlen. Druecke {0} zum Bestaetigen. Druecke {1} zum Zurueckgehen.",
                "Ayuda de resultados. Usa Arriba y Abajo para elegir una opcion. Pulsa {0} para confirmar. Pulsa {1} para volver.",
                "Ajuda de resultados. Use Cima e Baixo para escolher uma opcao. Pressione {0} para confirmar. Pressione {1} para voltar.");

            Add(
                "help_gameplay",
                "Gameplay help. Follow rhythm cues and press the announced actions. Press {0} to pause.",
                "游戏帮助。跟随节奏提示并按播报的动作键。按 {0} 暂停。",
                "遊戲說明。跟隨節奏提示並按語音播報的動作鍵。按 {0} 暫停。",
                "ゲームプレイヘルプ。リズムキューに従って指示された入力を押す。{0} で一時停止。",
                "게임플레이 도움말. 리듬 큐를 따라 안내된 동작을 누르세요. {0} 키로 일시정지합니다.",
                "Tro giup choi game. Theo cue nhip va nhan hanh dong duoc doc. Nhan {0} de tam dung.",
                "Aide gameplay. Suivez les reperes rythmiques et appuyez sur les actions annoncees. Appuyez sur {0} pour pause.",
                "Gameplay-Hilfe. Folge den Rhythmus-Hinweisen und druecke die angesagten Aktionen. Druecke {0} zum Pausieren.",
                "Ayuda de juego. Sigue las senales ritmicas y pulsa las acciones anunciadas. Pulsa {0} para pausar.",
                "Ajuda de jogo. Siga as dicas de ritmo e pressione as acoes anunciadas. Pressione {0} para pausar.");

            Add(
                "help_editor",
                "Editor help. Use arrows to move in the timeline. Press {0} to place or select. Press {1} to cancel.",
                "编辑器帮助。使用方向键在时间线上移动。按 {0} 放置或选择。按 {1} 取消。",
                "編輯器說明。使用方向鍵在時間軸上移動。按 {0} 放置或選擇。按 {1} 取消。",
                "エディターヘルプ。矢印キーでタイムラインを移動。{0} で配置または選択。{1} でキャンセル。",
                "에디터 도움말. 방향키로 타임라인을 이동합니다. {0} 키로 배치 또는 선택하고 {1} 키로 취소합니다.",
                "Tro giup trinh sua. Dung phim mui ten de di tren timeline. Nhan {0} de dat hoac chon. Nhan {1} de huy.",
                "Aide editeur. Utilisez les fleches pour bouger sur la timeline. Appuyez sur {0} pour placer ou choisir. Appuyez sur {1} pour annuler.",
                "Editor-Hilfe. Nutze Pfeiltasten zum Bewegen in der Timeline. Druecke {0} zum Platzieren oder Auswaehlen. Druecke {1} zum Abbrechen.",
                "Ayuda del editor. Usa flechas para moverte por la linea de tiempo. Pulsa {0} para colocar o seleccionar. Pulsa {1} para cancelar.",
                "Ajuda do editor. Use as setas para mover na linha do tempo. Pressione {0} para colocar ou selecionar. Pressione {1} para cancelar.");

            Add(
                "level_briefing_line",
                "{0}. {1}. {2}",
                "{0}。{1}。{2}",
                "{0}。{1}。{2}",
                "{0}。{1}。{2}",
                "{0}. {1}. {2}",
                "{0}. {1}. {2}",
                "{0}. {1}. {2}",
                "{0}. {1}. {2}",
                "{0}. {1}. {2}",
                "{0}. {1}. {2}");

            Add(
                "mode_practice",
                "Practice mode",
                "练习模式",
                "練習模式",
                "練習モード",
                "연습 모드",
                "Che do luyen tap",
                "Mode entrainement",
                "Uebungsmodus",
                "Modo practica",
                "Modo pratica");

            Add(
                "mode_score",
                "Score mode",
                "计分模式",
                "計分模式",
                "スコアモード",
                "점수 모드",
                "Che do tinh diem",
                "Mode score",
                "Punktemodus",
                "Modo puntuacion",
                "Modo pontuacao");

            Add(
                "mode_hard",
                "Hard mode",
                "困难模式",
                "困難模式",
                "ハードモード",
                "하드 모드",
                "Che do kho",
                "Mode difficile",
                "Schwerer Modus",
                "Modo dificil",
                "Modo dificil");

            Add(
                "mode_score_remix",
                "Score mode remix",
                "计分混音模式",
                "計分混音模式",
                "スコアリミックスモード",
                "점수 리믹스 모드",
                "Che do tinh diem remix",
                "Mode score remix",
                "Punktemodus Remix",
                "Modo puntuacion remix",
                "Modo pontuacao remix");

            Add(
                "mode_hard_remix",
                "Hard mode remix",
                "困难混音模式",
                "困難混音模式",
                "ハードリミックスモード",
                "하드 리믹스 모드",
                "Che do kho remix",
                "Mode difficile remix",
                "Schwerer Modus Remix",
                "Modo dificil remix",
                "Modo dificil remix");

            Add(
                "mode_tutorial",
                "Tutorial mode",
                "教程模式",
                "教學模式",
                "チュートリアルモード",
                "튜토리얼 모드",
                "Che do huong dan",
                "Mode tutoriel",
                "Tutorial-Modus",
                "Modo tutorial",
                "Modo tutorial");

            Add(
                "mode_editor_test",
                "Editor test mode",
                "编辑器测试模式",
                "編輯器測試模式",
                "エディターテストモード",
                "에디터 테스트 모드",
                "Che do thu editor",
                "Mode test editeur",
                "Editor-Testmodus",
                "Modo prueba editor",
                "Modo teste do editor");

            Add(
                "mode_community",
                "Community mode",
                "社区模式",
                "社群模式",
                "コミュニティモード",
                "커뮤니티 모드",
                "Che do cong dong",
                "Mode communaute",
                "Community-Modus",
                "Modo comunidad",
                "Modo comunidade");

            Add(
                "objective_default",
                "Follow rhythm cues and stay on beat.",
                "跟随节奏提示并保持节拍。",
                "跟隨節奏提示並保持節拍。",
                "リズムキューに従ってビートを保ってください。",
                "리듬 큐를 따라 비트를 유지하세요.",
                "Theo cue nhip va giu dung nhip.",
                "Suivez les reperes rythmiques et restez en rythme.",
                "Folge den Rhythmus-Hinweisen und bleibe im Takt.",
                "Sigue las senales ritmicas y manten el ritmo.",
                "Siga as dicas de ritmo e mantenha o tempo.");

            Add(
                "objective_practice",
                "Learn the pattern and timing. Press {0} to skip.",
                "学习节奏与时机。按 {0} 可跳过。",
                "學習節奏與時機。按 {0} 可跳過。",
                "パターンとタイミングを学びます。{0} でスキップ。",
                "패턴과 타이밍을 익히세요. {0} 키로 건너뛸 수 있습니다.",
                "Hoc mau nhip va thoi diem. Nhan {0} de bo qua.",
                "Apprenez le rythme et le timing. Appuyez sur {0} pour passer.",
                "Lerne Muster und Timing. Druecke {0}, um zu ueberspringen.",
                "Aprende el patron y el tiempo. Pulsa {0} para omitir.",
                "Aprenda o padrao e o tempo. Pressione {0} para pular.");

            Add(
                "objective_score",
                "Build score by timing actions on beat.",
                "在节拍上精准操作以提高分数。",
                "在節拍上精準操作以提高分數。",
                "ビートに合わせた入力でスコアを伸ばします。",
                "박자에 맞춘 입력으로 점수를 올리세요.",
                "Tang diem bang cach nhan dung theo nhip.",
                "Gagnez des points en entrant vos actions dans le rythme.",
                "Baue Punkte auf, indem du Aktionen im Takt eingibst.",
                "Gana puntuacion marcando acciones a tiempo.",
                "Aumente a pontuacao acertando as acoes no ritmo.");

            Add(
                "objective_hard",
                "Maintain accuracy on tighter patterns.",
                "在更紧凑的节奏中保持准确。",
                "在更緊湊的節奏中保持準確。",
                "より厳しいパターンで精度を維持します。",
                "더 촘촘한 패턴에서도 정확도를 유지하세요.",
                "Giu do chinh xac voi cac mau kho hon.",
                "Gardez une bonne precision sur des patterns plus exigeants.",
                "Halte Genauigkeit bei strengeren Mustern.",
                "Manten la precision en patrones mas exigentes.",
                "Mantenha a precisao em padroes mais exigentes.");

            Add(
                "objective_tutorial",
                "Follow instructions to learn mechanics.",
                "按照说明学习机制。",
                "按照說明學習機制。",
                "指示に従って仕組みを学びます。",
                "안내에 따라 기본 조작을 익히세요.",
                "Lam theo huong dan de hoc co che.",
                "Suivez les instructions pour apprendre les mecaniques.",
                "Folge den Anweisungen, um die Mechanik zu lernen.",
                "Sigue las instrucciones para aprender mecanicas.",
                "Siga as instrucoes para aprender as mecanicas.");

            Add(
                "objective_editor_test",
                "Test your chart timing and flow.",
                "测试你的谱面节奏与流程。",
                "測試你的譜面節奏與流程。",
                "譜面のタイミングと流れをテストします。",
                "차트의 타이밍과 흐름을 테스트하세요.",
                "Thu timing va nhip chay cua ban chart.",
                "Testez le timing et le flow de votre niveau.",
                "Teste Timing und Ablauf deiner Chart.",
                "Prueba el timing y flujo de tu nivel.",
                "Teste o timing e o fluxo do seu mapa.");

            Add(
                "objective_community",
                "Play and evaluate this community level.",
                "游玩并评价该社区关卡。",
                "遊玩並評價此社群關卡。",
                "このコミュニティレベルをプレイして評価します。",
                "커뮤니티 레벨을 플레이하고 평가하세요.",
                "Choi va danh gia man cong dong nay.",
                "Jouez et evaluez ce niveau communautaire.",
                "Spiele und bewerte dieses Community-Level.",
                "Juega y evalua este nivel de la comunidad.",
                "Jogue e avalie este nivel da comunidade.");

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
                "Perfect: {0}, Neujeum: {1}, Ppareum: {2}, Miss: {3}.",
                "Hoan hao: {0}, Tre: {1}, Som: {2}, Truot: {3}.",
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

            Add(
                "locked_requires_one_star",
                "Locked. Requires at least 1 star.",
                "已锁定。至少需要 1 星。",
                "已鎖定。至少需要 1 星。",
                "ロック中。少なくとも1つ星が必要です。",
                "잠김. 최소 별 1개가 필요합니다.",
                "Bi khoa. Can it nhat 1 sao.",
                "Verrouille. Necessite au moins 1 etoile.",
                "Gesperrt. Mindestens 1 Stern erforderlich.",
                "Bloqueado. Requiere al menos 1 estrella.",
                "Bloqueado. Requer pelo menos 1 estrela.");

            Add(
                "locked_requires_two_stars",
                "Locked. Requires 2 stars.",
                "已锁定。需要 2 星。",
                "已鎖定。需要 2 星。",
                "ロック中。2つ星が必要です。",
                "잠김. 별 2개가 필요합니다.",
                "Bi khoa. Can 2 sao.",
                "Verrouille. Necessite 2 etoiles.",
                "Gesperrt. 2 Sterne erforderlich.",
                "Bloqueado. Requiere 2 estrellas.",
                "Bloqueado. Requer 2 estrelas.");

            Add(
                "locked_requires_full_game",
                "Locked. Requires full game.",
                "已锁定。需要完整版游戏。",
                "已鎖定。需要完整版遊戲。",
                "ロック中。製品版が必要です。",
                "잠김. 정식 버전이 필요합니다.",
                "Bi khoa. Can ban day du cua tro choi.",
                "Verrouille. Necessite le jeu complet.",
                "Gesperrt. Vollversion erforderlich.",
                "Bloqueado. Requiere el juego completo.",
                "Bloqueado. Requer o jogo completo.");

            Add(
                "locked_requires_two_stars_full_game",
                "Locked. Requires 2 stars and full game.",
                "已锁定。需要 2 星和完整版游戏。",
                "已鎖定。需要 2 星與完整版遊戲。",
                "ロック中。2つ星と製品版が必要です。",
                "잠김. 별 2개와 정식 버전이 필요합니다.",
                "Bi khoa. Can 2 sao va ban day du cua tro choi.",
                "Verrouille. Necessite 2 etoiles et le jeu complet.",
                "Gesperrt. 2 Sterne und Vollversion erforderlich.",
                "Bloqueado. Requiere 2 estrellas y juego completo.",
                "Bloqueado. Requer 2 estrelas e o jogo completo.");
        }
    }
}
