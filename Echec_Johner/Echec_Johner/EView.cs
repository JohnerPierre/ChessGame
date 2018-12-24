using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Echec_Johner
{
    public partial class EView : Form
    {
        #region var
        EController _controller;
        Color colorWhite;
        Color colorBlack;
        Color colorGreen;
        Boolean movement = true;
        PictureBox pawnClick; // stocked last PiB for castling
        const int widthCase = 50;
        #endregion
        //constructor
        public EView()
        {
            InitializeComponent();
            _controller = new EController(this);
            // for futur for change color
            colorWhite = Color.Brown;
            colorBlack = Color.Beige;
            colorGreen = Color.Green;
        }

        private void EView_Load(object sender, EventArgs e)
        {
            _controller.CreatePowns();
            //For have transparancy gestion (for pown)
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            _controller.Load();
        }
        // In show beacause FormLoad is too early for Graphics
        private void EView_Shown(object sender, EventArgs e)
        {
            List<List<int>> mainList = new List<List<int>>();
            RefreshView(mainList);
            chessBoard.Enabled = false;
        }

        // called for each action on the chessboard
        private void RefreshView(List<List<int>> mainList)
        {
            _controller.RefreshView();
            if (mainList.Count == 0)
            {
                List<int> xList = new List<int>();
                List<int> yList = new List<int>();
                xList.Add(-10); yList.Add(-10);
                mainList.Add(xList); mainList.Add(yList);
            }
            Draw(mainList[0], mainList[1]);
            DisabledPawnsNotPlay();
            RefreshNumberOfMove();
        }
        // Draw chess board with lists of position colored
        public void Draw(List<int> X, List<int> Y)
        {
            Image bmp = new Bitmap(chessBoard.Width, chessBoard.Height);
            // create image
            using (Graphics g = Graphics.FromImage(bmp))
            {
                bool changeColor = true;
                bool token = true;
                SolidBrush blackBrush = new SolidBrush(colorWhite);
                SolidBrush whiteBrush = new SolidBrush(colorBlack);
                SolidBrush greenBrush = new SolidBrush(colorGreen);
                for (int y = 0; y <= 8; y++)
                {
                    token = true;
                    for (int x = 0; x <= 8; x++)
                    {
                        Rectangle rec = new Rectangle(x * widthCase, y * widthCase, widthCase, widthCase);
                        g.DrawRectangle(new Pen(Color.Black, 0), rec);
                        for (int z = 0; z < X.Count; z++)
                        {
                            // change color if pos is in the list
                            if (x + 1 == X[z] && y + 1 == Y[z])
                            {
                                g.FillRectangle(greenBrush, rec);
                                token = false;
                            }
                        }
                        if (token)
                        {
                            //alternate color to creat a checkboard
                            if (changeColor)
                            {
                                g.FillRectangle(whiteBrush, rec);

                            }
                            else
                            {
                                g.FillRectangle(blackBrush, rec);

                            }
                        }
                        token = true;
                        changeColor = !changeColor;
                    }
                }
            }
            // put image in the background of panel for gestion of transparancy
            chessBoard.BackgroundImage = bmp;
        }
        // gestion of number Move visually
        private void RefreshNumberOfMove()
        {
            lblNumberMovementBlack.Text = _controller.GetNumberOfMove("black");
            lblNumberMovementWhite.Text = _controller.GetNumberOfMove("white");
        }
        // gestion of player turn visually
        private void DisabledPawnsNotPlay()
        {
            foreach (PictureBox item in chessBoard.Controls)
            {
                // return color of team turn
                if (item.Name.Contains(_controller.CheckTurn()))
                    item.Enabled = true;
                else
                    item.Enabled = false;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _controller.ChangePause();
            CheckPause();
        }
        //gestion of pause visually
        private void CheckPause()
        {
            if (_controller.GetPause())
            {
                chessBoard.Enabled = false;
                btnPause.Text = "Reprendre";
            }
            else
            {
                chessBoard.Enabled = true;
                btnPause.Text = "Pause";
            }
        }

        // synchronize data position with view
        public void InvestmentPown(int x, int y, string name)
        {
            foreach (PictureBox control in chessBoard.Controls)
            {
                Point p = new Point(x * widthCase - widthCase, y * widthCase - widthCase);
                if (control.Name == name)
                    control.Location = p;
            }
        }
        // put pown dead in a other panel
        public void InvestementDeadPown(string name)
        {
            foreach (PictureBox item in chessBoard.Controls)
            {
                if (item.Name == name)
                {
                    if (name.Contains("Black"))
                    {
                        this.areaDeathBlack.Controls.Add(item);
                        this.chessBoard.Controls.Remove(item);
                        this.RefreshDeadZone(areaDeathBlack);
                    }
                    else
                    {
                        this.areaDeathWhite.Controls.Add(item);
                        this.chessBoard.Controls.Remove(item);
                        this.RefreshDeadZone(areaDeathWhite);
                    }
                }
            }
        }
        // place pawns in death panel
        private void RefreshDeadZone(Panel panel)
        {
            int countX = 0;
            int countY = 0;
            foreach (PictureBox item in panel.Controls)
            {
                // 3 is the number of pawns for each line
                if (countX % 3 == 0 && countX != 0)
                {
                    countY += widthCase;
                    countX = 0;
                }
                Point p = new Point(countX * widthCase, countY);
                item.Location = p;
                countX++;
            }
        }

        private void pown_MouseEnter(object sender, EventArgs e)
        {
            if (movement == true)
            {
                (sender as PictureBox).BackColor = Color.Red;
                // call GetPossibleMovement for have possible move of the pown
                List<List<int>> mainList = _controller.GetPossibleMovement((sender as PictureBox).Name, ((sender as PictureBox).Location.X + widthCase) / widthCase, ((sender as PictureBox).Location.Y + widthCase) / widthCase);
                RefreshView(mainList);
            }
        }
        private void pown_MouseLeave(object sender, EventArgs e)
        {
            // not refresh view when a pown was clicked
            if (movement == true)
            {
                RefreshView(new List<List<int>>());
            }
            (sender as PictureBox).BackColor = Color.Transparent;
        }
        private void pown_Click(object sender, EventArgs e)
        {
            List<List<int>> mainList = new List<List<int>>();
            (sender as PictureBox).BackColor = Color.Red;
            //  verification of castling
            if (((sender as PictureBox).Name.Contains("rook") && pawnClick.Name.Contains("king")) || ((sender as PictureBox).Name.Contains("king") && pawnClick.Name.Contains("rook")))
            {
                _controller.Castling(pawnClick.Name, (sender as PictureBox).Name);
                _controller.ChangeTurn();
                movement = true;
            }
            else
            {
                mainList = _controller.GetPossibleMovement((sender as PictureBox).Name, ((sender as PictureBox).Location.X + widthCase) / widthCase, ((sender as PictureBox).Location.Y + widthCase) / widthCase);
                movement = false;
                pawnClick = (sender as PictureBox);
            }
            RefreshView(mainList);
        }
        private void chessBoard_MouseDown(object sender, MouseEventArgs e)
        {
            // change pos if a pown was clicked
            if (movement == false)
            {
                bool move = false;
                move = _controller.ChangePos(pawnClick.Name, (e.X / widthCase + 1), (e.Y / widthCase + 1));
                List<List<int>> mainList = new List<List<int>>();
                _controller.CheckDeath(e.X / widthCase + 1, e.Y / widthCase + 1);
                if (move == true)
                    _controller.ChangeTurn();
                RefreshView(mainList);
                pawnClick = new PictureBox();
            }
            movement = true;
        }
        private void pown_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox tmpPb = sender as PictureBox;
        }

        private void nouvellePartieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckName();
            chessBoard.Enabled = true;
            timerPlay.Enabled = true;
            _controller.NewGame(txbNameB.Text, txbNameW.Text);
            foreach (PictureBox item in areaDeathBlack.Controls)
            {
                this.chessBoard.Controls.Add(item);
            }
            foreach (PictureBox item in areaDeathWhite.Controls)
            {
                this.chessBoard.Controls.Add(item);               
            }
            areaDeathBlack.Controls.Clear();
            areaDeathWhite.Controls.Clear();
            RefreshView(new List<List<int>>());
        }
        // if user has not name put a name for data
        private void CheckName()
        {
            string nameB = txbNameW.Text;
            string nameW = txbNameB.Text;
            if (nameB.Trim() == "")
            {
                nameB = "Joueur 1";
            }
            if (nameW.Trim() == "")
            {
                nameW = "Joueur 2";
            }
            txbNameW.Text = nameB;
            txbNameW.Enabled = false;
            txbNameB.Text = nameW;
            txbNameB.Enabled = false;
        }

        private void timerPlay_Tick(object sender, EventArgs e)
        {
            lblTimerBlack.Text = _controller.GetTime("black");
            lblTimerWhite.Text = _controller.GetTime("white");
            Checkmate();
        }
        //verification of checkmate
        private void Checkmate()
        {
            string colorCheckmate = _controller.Checkmate();
            // var is empty if not checkmate
            if (colorCheckmate != "")
            {
                if (colorCheckmate == "white")
                {
                    // lock pawns except king
                    foreach (PictureBox item in chessBoard.Controls)
                    {
                        item.Enabled = false;
                        if (item.Name.Contains("king") && item.Name.Contains("White"))
                            item.Enabled = true;
                    }
                }
                else
                {
                    // lock pawns except king
                    foreach (PictureBox item in chessBoard.Controls)
                    {
                        item.Enabled = false;
                        if (item.Name.Contains("king") && item.Name.Contains("Black"))
                            item.Enabled = true;
                    }
                }
            }
        }

        private void EView_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez vous sauvgarder?", "Message de confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                _controller.Save();
        }
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void sauvgarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Save();
        }
        private void reprendreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chessBoard.Enabled = true;
            timerPlay.Enabled = true;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBoxChess aboutBox = new AboutBoxChess();
            _controller.ChangePause();
            aboutBox.Show();
        }
    }
}
