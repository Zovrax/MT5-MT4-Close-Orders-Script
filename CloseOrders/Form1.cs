using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // ── Renkler ──────────────────────────────────────────────
        static readonly Color BgDark = Color.FromArgb(18, 20, 40);
        static readonly Color BgPanel = Color.FromArgb(25, 28, 55);
        static readonly Color BgCard = Color.FromArgb(30, 34, 62);
        static readonly Color BgCardInner = Color.FromArgb(22, 26, 48);
        static readonly Color AccentBlue = Color.FromArgb(63, 81, 181);
        static readonly Color AccentRed = Color.FromArgb(229, 57, 53);
        static readonly Color AccentGold = Color.FromArgb(251, 140, 0);
        static readonly Color AccentGreen = Color.FromArgb(46, 160, 67);
        static readonly Color TextMain = Color.FromArgb(230, 232, 255);
        static readonly Color TextSub = Color.FromArgb(140, 148, 200);
        static readonly Color TextGreen = Color.FromArgb(76, 175, 80);
        static readonly Color TextOrange = Color.FromArgb(255, 152, 0);
        static readonly Color BorderColor = Color.FromArgb(45, 50, 90);

        private Panel? pnlSidebar, pnlContent, pnlTop;
        private CheckBox? chkConfirm;
        private Button? btnRunBottom, btnRunHeader, btnDisconnect;
        private DataGridView? dgvOpen, dgvPending;
        private Label? lblConnStatus, lblConnDot, lblStatusBar;
        private ProgressBar? pgScript;
        private System.Windows.Forms.Timer? tmrScript;
        private int scriptStep = 0;
        private bool isConnected = true;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            BuildSidebar();
            BuildTopBar();
            BuildContent();
            BuildStatusBar();
        }

        void SetupForm()
        {
            this.Text = "Auto Scripts – MT5 Script Manager";
            this.Size = new Size(1300, 920);
            this.MinimumSize = new Size(900, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BgDark;
            this.ForeColor = TextMain;
            this.Font = new Font("Segoe UI", 9.5f);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        // ════════════════════════════════════════════════════════
        void BuildTopBar()
        {
            pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 52;
            pnlTop.BackColor = Color.FromArgb(22, 24, 50);
            this.Controls.Add(pnlTop);
            pnlTop.BringToFront();

            Label btnMenu = new Label();
            btnMenu.Text = "☰";
            btnMenu.Font = new Font("Segoe UI", 14f);
            btnMenu.ForeColor = TextMain;
            btnMenu.AutoSize = false;
            btnMenu.Size = new Size(44, 52);
            btnMenu.Location = new Point(8, 0);
            btnMenu.TextAlign = ContentAlignment.MiddleCenter;
            btnMenu.Cursor = Cursors.Hand;
            pnlTop.Controls.Add(btnMenu);

            Label lblTitle = new Label();
            lblTitle.Text = "Auto Scripts";
            lblTitle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            lblTitle.ForeColor = TextMain;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(58, 8);
            pnlTop.Controls.Add(lblTitle);

            Label lblSub = new Label();
            lblSub.Text = "MT5 Script Manager";
            lblSub.Font = new Font("Segoe UI", 8.5f);
            lblSub.ForeColor = TextSub;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(60, 30);
            pnlTop.Controls.Add(lblSub);

            // Kapat butonu — hover kırmızı
            Button btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.Size = new Size(46, 52);
            btnClose.Location = new Point(this.Width - 46, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.BackColor = Color.Transparent;
            btnClose.ForeColor = Color.FromArgb(160, 160, 200);
            btnClose.Font = new Font("Segoe UI", 11f);
            btnClose.Cursor = Cursors.Hand;
            btnClose.TabStop = false;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.MouseEnter += (s, e) => { btnClose.BackColor = AccentRed; btnClose.ForeColor = Color.White; };
            btnClose.MouseLeave += (s, e) => { btnClose.BackColor = Color.Transparent; btnClose.ForeColor = Color.FromArgb(160, 160, 200); };
            btnClose.Click += (s, e) => Application.Exit();
            pnlTop.Controls.Add(btnClose);

            Button btnMax = MakeWinBtn("□", new Point(this.Width - 92, 14));
            Button btnMin = MakeWinBtn("─", new Point(this.Width - 138, 14));
            btnMin.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            btnMax.Click += (s, e) => this.WindowState =
                (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
            pnlTop.Controls.Add(btnMax);
            pnlTop.Controls.Add(btnMin);

            this.Resize += (s, e) =>
            {
                btnClose.Left = this.Width - 46;
                btnMax.Left = this.Width - 92;
                btnMin.Left = this.Width - 138;
            };

            bool drag = false;
            Point dragStart = Point.Empty;
            pnlTop.MouseDown += (s, e) => { drag = true; dragStart = e.Location; };
            pnlTop.MouseUp += (s, e) => { drag = false; };
            pnlTop.MouseMove += (s, e) =>
            {
                if (drag) this.Location = new Point(
                    this.Location.X + e.X - dragStart.X,
                    this.Location.Y + e.Y - dragStart.Y);
            };
        }

        Button MakeWinBtn(string txt, Point loc)
        {
            Button b = new Button();
            b.Text = txt;
            b.Size = new Size(30, 24);
            b.Location = loc;
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Color.FromArgb(50, 55, 90);
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 8f);
            b.Cursor = Cursors.Hand;
            b.TabStop = false;
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // ════════════════════════════════════════════════════════
        void BuildSidebar()
        {
            pnlSidebar = new Panel();
            pnlSidebar.Width = 210;
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.BackColor = BgPanel;
            this.Controls.Add(pnlSidebar);

            var items = new (string icon, string label, bool active)[]
            {
                ("🗂", "Library",   false),
                ("🏠", "Home",      true),
                ("📊", "Dashboard", false),
                ("📅", "Events",    false),
                ("ℹ",  "About Us",  false),
                ("✉",  "Contact",   false),
            };

            int y = 70;
            foreach (var item in items)
            {
                Panel row = new Panel();
                row.Location = new Point(0, y);
                row.Size = new Size(210, 44);
                row.BackColor = item.active ? AccentBlue : Color.Transparent;
                row.Cursor = Cursors.Hand;
                row.MouseEnter += (s, e) => { if (!item.active) row.BackColor = Color.FromArgb(35, 40, 75); };
                row.MouseLeave += (s, e) => { row.BackColor = item.active ? AccentBlue : Color.Transparent; };

                Label ico = new Label();
                ico.Text = item.icon;
                ico.Font = new Font("Segoe UI", 11f);
                ico.ForeColor = item.active ? Color.White : TextSub;
                ico.AutoSize = false;
                ico.Size = new Size(36, 44);
                ico.Location = new Point(14, 0);
                ico.TextAlign = ContentAlignment.MiddleCenter;

                Label lbl = new Label();
                lbl.Text = item.label;
                lbl.Font = new Font("Segoe UI", item.active ? 9.5f : 9f,
                                         item.active ? FontStyle.Bold : FontStyle.Regular);
                lbl.ForeColor = item.active ? Color.White : TextSub;
                lbl.AutoSize = false;
                lbl.Size = new Size(140, 44);
                lbl.Location = new Point(52, 0);
                lbl.TextAlign = ContentAlignment.MiddleLeft;

                row.Controls.Add(ico);
                row.Controls.Add(lbl);
                pnlSidebar.Controls.Add(row);
                y += 48;
            }

            // ── Bağlantı paneli ──
            Panel pnlConn = new Panel();
            pnlConn.Size = new Size(210, 140);
            pnlConn.BackColor = Color.Transparent;
            pnlConn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlConn.Location = new Point(0, pnlSidebar.Height - 148);
            pnlSidebar.Controls.Add(pnlConn);
            pnlSidebar.Resize += (s, e) => pnlConn.Top = pnlSidebar.Height - 148;

            lblConnDot = new Label();
            lblConnDot.Text = "●";
            lblConnDot.Font = new Font("Segoe UI", 10f);
            lblConnDot.ForeColor = TextGreen;
            lblConnDot.AutoSize = true;
            lblConnDot.Location = new Point(14, 8);

            Label lblMT5 = new Label();
            lblMT5.Text = "MT5 Connection";
            lblMT5.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblMT5.ForeColor = TextMain;
            lblMT5.AutoSize = true;
            lblMT5.Location = new Point(30, 8);

            lblConnStatus = new Label();
            lblConnStatus.Text = "Connected";
            lblConnStatus.Font = new Font("Segoe UI", 8.5f);
            lblConnStatus.ForeColor = TextGreen;
            lblConnStatus.AutoSize = true;
            lblConnStatus.Location = new Point(30, 26);

            Label lblSrv = new Label();
            lblSrv.Text = "Server:  MetaQuotes-Demo";
            lblSrv.Font = new Font("Segoe UI", 8f);
            lblSrv.ForeColor = TextSub;
            lblSrv.AutoSize = true;
            lblSrv.Location = new Point(14, 46);

            Label lblLgn = new Label();
            lblLgn.Text = "Login:  12345678";
            lblLgn.Font = new Font("Segoe UI", 8f);
            lblLgn.ForeColor = TextSub;
            lblLgn.AutoSize = true;
            lblLgn.Location = new Point(14, 62);

            btnDisconnect = new Button();
            btnDisconnect.Text = "⏻  Disconnect";
            btnDisconnect.Location = new Point(12, 84);
            btnDisconnect.Size = new Size(182, 30);
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.BackColor = Color.FromArgb(35, 40, 72);
            btnDisconnect.ForeColor = TextSub;
            btnDisconnect.Font = new Font("Segoe UI", 8.5f);
            btnDisconnect.Cursor = Cursors.Hand;
            btnDisconnect.TabStop = false;
            btnDisconnect.FlatAppearance.BorderColor = BorderColor;
            btnDisconnect.Click += OnDisconnectClick;

            pnlConn.Controls.Add(lblConnDot);
            pnlConn.Controls.Add(lblMT5);
            pnlConn.Controls.Add(lblConnStatus);
            pnlConn.Controls.Add(lblSrv);
            pnlConn.Controls.Add(lblLgn);
            pnlConn.Controls.Add(btnDisconnect);

            Panel pnlVer = new Panel();
            pnlVer.Dock = DockStyle.Bottom;
            pnlVer.Height = 20;
            pnlVer.BackColor = Color.Transparent;
            Label lblVer = new Label();
            lblVer.Text = "v1.0.0";
            lblVer.Font = new Font("Segoe UI", 8f);
            lblVer.ForeColor = Color.FromArgb(70, 80, 120);
            lblVer.AutoSize = true;
            lblVer.Location = new Point(14, 2);
            pnlVer.Controls.Add(lblVer);
            pnlSidebar.Controls.Add(pnlVer);
        }

        // ════════════════════════════════════════════════════════
        void BuildContent()
        {
            pnlContent = new Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = BgDark;
            pnlContent.AutoScroll = true;
            pnlContent.Padding = new Padding(24, 20, 24, 24);
            this.Controls.Add(pnlContent);
            pnlContent.BringToFront();

            // ── Header ──────────────────────────────────────
            Panel pnlHeader = Card(new Point(0, 0), new Size(pnlContent.Width - 48, 90));
            pnlContent.Controls.Add(pnlHeader);
            pnlContent.Resize += (s, e) => pnlHeader.Width = pnlContent.Width - 48;

            Panel pnlIcon = new Panel();
            pnlIcon.Location = new Point(16, 14);
            pnlIcon.Size = new Size(62, 62);
            pnlIcon.BackColor = Color.FromArgb(200, 50, 50);
            Label lblX = new Label();
            lblX.Text = "✕";
            lblX.Font = new Font("Segoe UI", 22f, FontStyle.Bold);
            lblX.ForeColor = Color.White;
            lblX.AutoSize = false;
            lblX.Size = new Size(62, 62);
            lblX.TextAlign = ContentAlignment.MiddleCenter;
            pnlIcon.Controls.Add(lblX);
            pnlHeader.Controls.Add(pnlIcon);

            Label lblScriptName = new Label();
            lblScriptName.Text = "Close All Orders Script";
            lblScriptName.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblScriptName.ForeColor = TextMain;
            lblScriptName.AutoSize = true;
            lblScriptName.Location = new Point(94, 14);
            pnlHeader.Controls.Add(lblScriptName);

            Label lblScriptDesc = new Label();
            lblScriptDesc.Text = "All open positions and pending orders will be closed instantly.";
            lblScriptDesc.Font = new Font("Segoe UI", 9f);
            lblScriptDesc.ForeColor = TextSub;
            lblScriptDesc.AutoSize = true;
            lblScriptDesc.Location = new Point(94, 44);
            pnlHeader.Controls.Add(lblScriptDesc);

            btnRunHeader = RoundButton("▶  Run Script", AccentRed, new Point(pnlHeader.Width - 200, 16), new Size(170, 34));
            pnlHeader.Controls.Add(btnRunHeader);
            pnlHeader.Resize += (s, e) => { if (btnRunHeader != null) btnRunHeader.Left = pnlHeader.Width - 200; };

            Button btnSettings = RoundButton("⚙  Script Settings", Color.FromArgb(35, 40, 72), new Point(pnlHeader.Width - 200, 58), new Size(170, 24));
            btnSettings.ForeColor = TextSub;
            btnSettings.FlatAppearance.BorderColor = BorderColor;
            pnlHeader.Controls.Add(btnSettings);
            pnlHeader.Resize += (s, e) => btnSettings.Left = pnlHeader.Width - 200;

            // Progress bar (başta gizli)
            pgScript = new ProgressBar();
            pgScript.Location = new Point(0, 88);
            pgScript.Size = new Size(pnlHeader.Width, 4);
            pgScript.Minimum = 0;
            pgScript.Maximum = 100;
            pgScript.Value = 0;
            pgScript.Style = ProgressBarStyle.Continuous;
            pgScript.Visible = false;
            pnlHeader.Controls.Add(pgScript);
            pnlHeader.Resize += (s, e) => { if (pgScript != null) pgScript.Width = pnlHeader.Width; };

            // ── Info + What kartları ──────────────────────────
            int cardY = 106;
            Panel pnlInfo = Card(new Point(0, cardY), new Size(500, 280));
            Panel pnlWhat = Card(new Point(514, cardY), new Size(pnlContent.Width - 562, 280));
            pnlContent.Controls.Add(pnlInfo);
            pnlContent.Controls.Add(pnlWhat);
            pnlContent.Resize += (s, e) => pnlWhat.Width = pnlContent.Width - 562;

            SectionTitle(pnlInfo, "Script Information", 14, 14);
            string[,] info = {
                { "Name:",        "Close All Orders Script" },
                { "Description:", "Closes all open positions and\npending orders on the MT5 account." },
                { "Type:",        "Utility Script" },
                { "Platform:",    "MT5" },
                { "Version:",     "1.0.0" },
                { "Author:",      "Auto Scripts Team" },
            };
            int iy = 46;
            for (int r = 0; r < info.GetLength(0); r++)
            {
                Label lk = new Label();
                lk.Text = info[r, 0];
                lk.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                lk.ForeColor = TextMain;
                lk.AutoSize = true;
                lk.Location = new Point(14, iy);
                pnlInfo.Controls.Add(lk);

                if (info[r, 0] == "Platform:")
                {
                    Label badge = new Label();
                    badge.Text = "MT5";
                    badge.Font = new Font("Segoe UI", 8f);
                    badge.ForeColor = Color.White;
                    badge.AutoSize = false;
                    badge.Size = new Size(36, 20);
                    badge.Location = new Point(160, iy);
                    badge.TextAlign = ContentAlignment.MiddleCenter;
                    badge.BackColor = AccentBlue;
                    pnlInfo.Controls.Add(badge);
                }
                else
                {
                    Label lv = new Label();
                    lv.Text = info[r, 1];
                    lv.Font = new Font("Segoe UI", 8.5f);
                    lv.ForeColor = TextSub;
                    lv.AutoSize = false;
                    lv.Size = new Size(300, 36);
                    lv.Location = new Point(160, iy);
                    pnlInfo.Controls.Add(lv);
                }
                iy += (info[r, 0] == "Description:") ? 48 : 30;
            }

            SectionTitle(pnlWhat, "What This Script Does", 14, 14);
            string[] feats = {
                "Closes all open market positions",
                "Cancels all pending orders",
                "Works on all symbols",
                "Requires confirmation before execution"
            };
            int wy = 50;
            foreach (string f in feats)
            {
                Label dot = new Label(); dot.Text = "✔"; dot.Font = new Font("Segoe UI", 10f); dot.ForeColor = TextGreen; dot.AutoSize = true; dot.Location = new Point(14, wy);
                Label lf = new Label(); lf.Text = f; lf.Font = new Font("Segoe UI", 9f); lf.ForeColor = TextSub; lf.AutoSize = true; lf.Location = new Point(38, wy);
                pnlWhat.Controls.Add(dot);
                pnlWhat.Controls.Add(lf);
                wy += 34;
            }

            // ── Preview kartı ─────────────────────────────────
            int preY = cardY + 296;
            Panel pnlPreview = Card(new Point(0, preY), new Size(pnlContent.Width - 48, 290));
            pnlContent.Controls.Add(pnlPreview);
            pnlContent.Resize += (s, e) => pnlPreview.Width = pnlContent.Width - 48;
            SectionTitle(pnlPreview, "Preview  (What will be closed)", 14, 14);

            Panel pnlOpen = InnerCard(pnlPreview, new Point(14, 46), new Size(490, 198));
            BadgeTitle(pnlOpen, "Open Positions", "2", AccentBlue, 12, 10);
            dgvOpen = MakeGrid(pnlOpen, new Point(12, 40), new Size(464, 110),
                new string[] { "Symbol", "Type", "Volume", "Price", "P/L" },
                new string[][] {
                    new string[] { "EURUSD", "Buy",  "0.10", "1.08520", "+$12.45" },
                    new string[] { "XAUUSD", "Sell", "0.05", "2345.10",  "-$8.30" }
                });
            dgvOpen.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == 4 && e.Value != null)
                {
                    string v = e.Value.ToString() ?? "";
                    if (v.StartsWith("+")) e.CellStyle.ForeColor = TextGreen;
                    else if (v == "Closed") e.CellStyle.ForeColor = TextSub;
                    else e.CellStyle.ForeColor = AccentRed;
                }
            };
            Label lblTotalO = new Label(); lblTotalO.Text = "Total Positions: 2       Total Volume: 0.15"; lblTotalO.Font = new Font("Segoe UI", 8f); lblTotalO.ForeColor = TextSub; lblTotalO.AutoSize = true; lblTotalO.Location = new Point(12, 158);
            pnlOpen.Controls.Add(lblTotalO);

            int pw = Math.Max(pnlPreview.Width - 530, 200);
            Panel pnlPend = InnerCard(pnlPreview, new Point(516, 46), new Size(pw, 198));
            pnlPreview.Resize += (s, e) => pnlPend.Width = Math.Max(pnlPreview.Width - 530, 200);
            BadgeTitle(pnlPend, "Pending Orders", "1", AccentGold, 12, 10);
            dgvPending = MakeGrid(pnlPend, new Point(12, 40), new Size(pnlPend.Width - 24, 110),
                new string[] { "Symbol", "Type", "Volume", "Price", "Status" },
                new string[][] {
                    new string[] { "GBPUSD", "Buy Limit", "0.10", "1.27000", "Pending" }
                });
            dgvPending.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == 4 && e.Value != null)
                {
                    string v = e.Value.ToString() ?? "";
                    if (v == "Pending") e.CellStyle.ForeColor = TextOrange;
                    else if (v == "Cancelled") e.CellStyle.ForeColor = AccentRed;
                }
            };
            pnlPend.Resize += (s, e) => { if (dgvPending != null) dgvPending.Width = pnlPend.Width - 24; };
            Label lblTotalP = new Label(); lblTotalP.Text = "Total Pending Orders: 1"; lblTotalP.Font = new Font("Segoe UI", 8f); lblTotalP.ForeColor = TextSub; lblTotalP.AutoSize = true; lblTotalP.Location = new Point(12, 158);
            pnlPend.Controls.Add(lblTotalP);

            // ── Warning + Confirmation ─────────────────────────
            int warnY = preY + 306;
            Panel pnlWarn = Card(new Point(0, warnY), new Size(pnlContent.Width - 48, 82));
            pnlContent.Controls.Add(pnlWarn);
            pnlContent.Resize += (s, e) => pnlWarn.Width = pnlContent.Width - 48;

            Label lblWarnTitle = new Label(); lblWarnTitle.Text = "⚠  Warning"; lblWarnTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold); lblWarnTitle.ForeColor = AccentRed; lblWarnTitle.AutoSize = true; lblWarnTitle.Location = new Point(14, 12);
            Label lblWarnText = new Label(); lblWarnText.Text = "This action cannot be undone. All open positions and pending orders will be permanently closed/canceled."; lblWarnText.Font = new Font("Segoe UI", 8.5f); lblWarnText.ForeColor = TextSub; lblWarnText.AutoSize = false; lblWarnText.Size = new Size(480, 40); lblWarnText.Location = new Point(14, 32);
            pnlWarn.Controls.Add(lblWarnTitle);
            pnlWarn.Controls.Add(lblWarnText);

            Label lblConfTitle = new Label(); lblConfTitle.Text = "Confirmation Required"; lblConfTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold); lblConfTitle.ForeColor = TextMain; lblConfTitle.AutoSize = true; lblConfTitle.Location = new Point(pnlWarn.Width / 2, 12);
            pnlWarn.Controls.Add(lblConfTitle);
            pnlWarn.Resize += (s, e) => lblConfTitle.Left = pnlWarn.Width / 2;

            chkConfirm = new CheckBox();
            chkConfirm.Text = "I understand that this will close all positions and cancel all pending orders.";
            chkConfirm.Font = new Font("Segoe UI", 8.5f);
            chkConfirm.ForeColor = TextSub;
            chkConfirm.AutoSize = false;
            chkConfirm.Size = new Size(460, 36);
            chkConfirm.Location = new Point(pnlWarn.Width / 2, 32);
            chkConfirm.BackColor = Color.Transparent;
            pnlWarn.Controls.Add(chkConfirm);
            pnlWarn.Resize += (s, e) => { if (chkConfirm != null) chkConfirm.Left = pnlWarn.Width / 2; };

            btnRunBottom = RoundButton("▶  Run Script", Color.FromArgb(60, 65, 100), new Point(pnlWarn.Width - 180, 24), new Size(160, 34));
            btnRunBottom.ForeColor = Color.FromArgb(100, 110, 160);
            btnRunBottom.Enabled = false;
            pnlWarn.Controls.Add(btnRunBottom);
            pnlWarn.Resize += (s, e) => { if (btnRunBottom != null) btnRunBottom.Left = pnlWarn.Width - 180; };

            chkConfirm.CheckedChanged += (s, e) =>
            {
                if (btnRunBottom == null || chkConfirm == null) return;
                bool c = chkConfirm.Checked;
                btnRunBottom.Enabled = c;
                btnRunBottom.BackColor = c ? AccentRed : Color.FromArgb(60, 65, 100);
                btnRunBottom.ForeColor = c ? Color.White : Color.FromArgb(100, 110, 160);
            };

            btnRunHeader.Click += OnRunScriptClick;
            btnRunBottom.Click += OnRunScriptClick;

            pnlContent.AutoScrollMinSize = new Size(0, warnY + 130);
        }

        // ════════════════════════════════════════════════════════
        void BuildStatusBar()
        {
            Panel pnlSB = new Panel();
            pnlSB.Dock = DockStyle.Bottom;
            pnlSB.Height = 26;
            pnlSB.BackColor = Color.FromArgb(15, 17, 35);
            this.Controls.Add(pnlSB);
            pnlSB.BringToFront();

            lblStatusBar = new Label();
            lblStatusBar.Text = "  Ready.";
            lblStatusBar.Font = new Font("Segoe UI", 8f);
            lblStatusBar.ForeColor = TextSub;
            lblStatusBar.AutoSize = false;
            lblStatusBar.Size = new Size(600, 26);
            lblStatusBar.Location = new Point(0, 0);
            lblStatusBar.TextAlign = ContentAlignment.MiddleLeft;
            pnlSB.Controls.Add(lblStatusBar);

            Label lblTime = new Label();
            lblTime.Font = new Font("Segoe UI", 8f);
            lblTime.ForeColor = Color.FromArgb(80, 90, 130);
            lblTime.AutoSize = false;
            lblTime.Size = new Size(160, 26);
            lblTime.TextAlign = ContentAlignment.MiddleRight;
            lblTime.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            lblTime.Location = new Point(this.Width - 374, 0);
            pnlSB.Controls.Add(lblTime);
            this.Resize += (s, e) => lblTime.Left = this.Width - 374;

            System.Windows.Forms.Timer tmrClock = new System.Windows.Forms.Timer();
            tmrClock.Interval = 1000;
            tmrClock.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("HH:mm:ss  dd.MM.yyyy");
            tmrClock.Start();
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss  dd.MM.yyyy");
        }

        // ════════════════════════════════════════════════════════
        // Olay işleyicileri
        // ════════════════════════════════════════════════════════

        void OnRunScriptClick(object? sender, EventArgs e)
        {
            if (!isConnected)
            {
                SetStatus("⚠  Cannot run — MT5 is disconnected!", AccentRed);
                return;
            }
            if (chkConfirm != null && !chkConfirm.Checked)
            {
                SetStatus("⚠  Please check the confirmation box first.", TextOrange);
                return;
            }

            if (btnRunHeader != null) { btnRunHeader.Enabled = false; btnRunHeader.BackColor = Color.FromArgb(40, 100, 60); btnRunHeader.Text = "⏳  Running..."; }
            if (btnRunBottom != null) { btnRunBottom.Enabled = false; btnRunBottom.BackColor = Color.FromArgb(40, 100, 60); btnRunBottom.Text = "⏳  Running..."; }
            if (chkConfirm != null) chkConfirm.Enabled = false;
            if (pgScript != null) { pgScript.Value = 0; pgScript.Visible = true; }

            scriptStep = 0;
            SetStatus("⏳  Script started — connecting to MT5...", TextOrange);

            tmrScript = new System.Windows.Forms.Timer();
            tmrScript.Interval = 700;
            tmrScript.Tick += ScriptTick;
            tmrScript.Start();
        }

        void ScriptTick(object? sender, EventArgs e)
        {
            scriptStep++;
            if (pgScript != null) pgScript.Value = Math.Min(scriptStep * 20, 100);

            switch (scriptStep)
            {
                case 1: SetStatus("⏳  Closing open positions...", TextOrange); break;
                case 2:
                    SetStatus("⏳  Cancelling pending orders...", TextOrange);
                    if (dgvOpen != null && dgvOpen.Rows.Count > 0) dgvOpen.Rows[0].Cells[4].Value = "Closed";
                    break;
                case 3:
                    SetStatus("⏳  Verifying closure...", TextOrange);
                    if (dgvOpen != null && dgvOpen.Rows.Count > 1) dgvOpen.Rows[1].Cells[4].Value = "Closed";
                    if (dgvPending != null && dgvPending.Rows.Count > 0) dgvPending.Rows[0].Cells[4].Value = "Cancelled";
                    break;
                case 4: SetStatus("⏳  Finalizing...", TextOrange); break;
                case 5:
                    tmrScript?.Stop();
                    tmrScript?.Dispose();
                    tmrScript = null;
                    if (pgScript != null) pgScript.Visible = false;

                    if (btnRunHeader != null) { btnRunHeader.BackColor = AccentGreen; btnRunHeader.Text = "✓  Completed"; btnRunHeader.ForeColor = Color.White; }
                    if (btnRunBottom != null) { btnRunBottom.BackColor = AccentGreen; btnRunBottom.Text = "✓  Completed"; btnRunBottom.ForeColor = Color.White; }

                    SetStatus("✅  Script completed — all orders closed.", TextGreen);

                    System.Windows.Forms.Timer tmrReset = new System.Windows.Forms.Timer();
                    tmrReset.Interval = 3000;
                    tmrReset.Tick += (s2, e2) =>
                    {
                        tmrReset.Stop();
                        if (btnRunHeader != null) { btnRunHeader.BackColor = AccentRed; btnRunHeader.Text = "▶  Run Script"; btnRunHeader.Enabled = true; }
                        if (btnRunBottom != null) { btnRunBottom.BackColor = Color.FromArgb(60, 65, 100); btnRunBottom.Text = "▶  Run Script"; btnRunBottom.ForeColor = Color.FromArgb(100, 110, 160); }
                        if (chkConfirm != null) { chkConfirm.Enabled = true; chkConfirm.Checked = false; }
                        if (dgvOpen != null && dgvOpen.Rows.Count > 1) { dgvOpen.Rows[0].Cells[4].Value = "+$12.45"; dgvOpen.Rows[1].Cells[4].Value = "-$8.30"; }
                        if (dgvPending != null && dgvPending.Rows.Count > 0) dgvPending.Rows[0].Cells[4].Value = "Pending";
                        SetStatus("  Ready.", TextSub);
                    };
                    tmrReset.Start();
                    break;
            }
        }

        void OnDisconnectClick(object? sender, EventArgs e)
        {
            isConnected = !isConnected;
            if (lblConnDot != null) lblConnDot.ForeColor = isConnected ? TextGreen : AccentRed;
            if (lblConnStatus != null) { lblConnStatus.Text = isConnected ? "Connected" : "Disconnected"; lblConnStatus.ForeColor = isConnected ? TextGreen : AccentRed; }
            if (btnDisconnect != null) btnDisconnect.Text = isConnected ? "⏻  Disconnect" : "⏻  Connect";
            if (btnRunHeader != null) btnRunHeader.Enabled = isConnected;
            SetStatus(isConnected ? "✅  MT5 connected — MetaQuotes-Demo." : "🔴  MT5 disconnected.", isConnected ? TextGreen : AccentRed);
        }

        void SetStatus(string msg, Color color)
        {
            if (lblStatusBar == null) return;
            lblStatusBar.Text = "  " + msg;
            lblStatusBar.ForeColor = color;
        }

        // ════════════════════════════════════════════════════════
        Panel Card(Point loc, Size sz)
        {
            Panel p = new Panel();
            p.Location = loc; p.Size = sz; p.BackColor = BgCard;
            p.Paint += (s, e) => { Pen pen = new Pen(BorderColor, 1); e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1); pen.Dispose(); };
            return p;
        }

        Panel InnerCard(Panel parent, Point loc, Size sz)
        {
            Panel p = new Panel();
            p.Location = loc; p.Size = sz; p.BackColor = BgCardInner;
            p.Paint += (s, e) => { Pen pen = new Pen(BorderColor, 1); e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1); pen.Dispose(); };
            parent.Controls.Add(p);
            return p;
        }

        void SectionTitle(Panel parent, string text, int x, int y)
        {
            Label lbl = new Label(); lbl.Text = text; lbl.Font = new Font("Segoe UI", 10f, FontStyle.Bold); lbl.ForeColor = TextMain; lbl.AutoSize = true; lbl.Location = new Point(x, y);
            parent.Controls.Add(lbl);
        }

        void BadgeTitle(Panel parent, string title, string badge, Color badgeColor, int x, int y)
        {
            Label lbl = new Label(); lbl.Text = title; lbl.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); lbl.ForeColor = TextMain; lbl.AutoSize = true; lbl.Location = new Point(x, y);
            Label bdg = new Label(); bdg.Text = badge; bdg.Font = new Font("Segoe UI", 8f, FontStyle.Bold); bdg.ForeColor = Color.White; bdg.AutoSize = false; bdg.Size = new Size(22, 18); bdg.Location = new Point(x + 130, y + 1); bdg.TextAlign = ContentAlignment.MiddleCenter; bdg.BackColor = badgeColor;
            parent.Controls.Add(lbl); parent.Controls.Add(bdg);
        }

        Button RoundButton(string text, Color bg, Point loc, Size sz)
        {
            Button b = new Button(); b.Text = text; b.Location = loc; b.Size = sz; b.FlatStyle = FlatStyle.Flat; b.BackColor = bg; b.ForeColor = Color.White; b.Font = new Font("Segoe UI", 9f, FontStyle.Bold); b.Cursor = Cursors.Hand; b.TabStop = false; b.FlatAppearance.BorderSize = 0;
            return b;
        }

        DataGridView MakeGrid(Panel parent, Point loc, Size sz, string[] cols, string[][] rows)
        {
            DataGridView grid = new DataGridView();
            grid.Location = loc; grid.Size = sz; grid.BackgroundColor = BgCardInner; grid.GridColor = BorderColor; grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false; grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; grid.ColumnHeadersHeight = 28;
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.ReadOnly = true; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grid.EnableHeadersVisualStyles = false;
            DataGridViewCellStyle cell = new DataGridViewCellStyle(); cell.BackColor = BgCardInner; cell.ForeColor = TextSub; cell.Font = new Font("Segoe UI", 8.5f); cell.SelectionBackColor = AccentBlue; cell.SelectionForeColor = Color.White; grid.DefaultCellStyle = cell;
            DataGridViewCellStyle hdr = new DataGridViewCellStyle(); hdr.BackColor = BgCard; hdr.ForeColor = TextSub; hdr.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold); hdr.SelectionBackColor = BgCard; grid.ColumnHeadersDefaultCellStyle = hdr;
            foreach (string c in cols) grid.Columns.Add(c, c);
            foreach (string[] r in rows) grid.Rows.Add(r);
            parent.Controls.Add(grid);
            return grid;
        }
    }
}