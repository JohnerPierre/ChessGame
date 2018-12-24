using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Echec_Johner
{
    // class for easly gestion of position data
    class PositionsBag
    {
        List<int> _xList = new List<int>();
        List<int> _yList = new List<int>();
        public List<int> XList
        {
            get { return _xList; }
            set { _xList = value; }
        }
        public List<int> YList
        {
            get { return _yList; }
            set { _yList = value; }
        }
        public PositionsBag()
        {
        }
        public void AddTwoBags(PositionsBag bagOne, PositionsBag bagTwo)
        {
            XList.AddRange(bagOne.XList);
            XList.AddRange(bagTwo.XList);
            YList.AddRange(bagOne.YList);
            YList.AddRange(bagTwo.YList);
        }
        public void AddBag(PositionsBag bag)
        {
            XList.AddRange(bag.XList);
            YList.AddRange(bag.YList);
        }
    }

    [System.Serializable]
    class EModel
    {
        #region var

        List<Pown> _pawns;
        bool _turn; // true = white false = black
        bool _pause;
        int _timeWhite;//[s]
        int _timeBlack;
        int _numberMovementW;
        int _numberMovementB;
        string _nameW;
        string _nameB;
        Timer _timeTimer;

        const string NameFileSave = "play.chess";

        EController _controller;

        #endregion
        #region Getter/Setter

        public bool Pause
        {
            get { return _pause; }
            set { _pause = value; }
        }
        private Timer TimeTimer
        {
            get { return _timeTimer; }
            set { _timeTimer = value; }
        }
        public List<Pown> Pawns
        {
            get { return _pawns; }
            set { _pawns = value; }
        }
        private bool Turn
        {
            get { return _turn; }
            set { _turn = value; }
        }
        private int TimeWhite
        {
            get { return _timeWhite; }
            set { _timeWhite = value; }
        }
        private int TimeBlack
        {
            get { return _timeBlack; }
            set { _timeBlack = value; }
        }
        private int NumberMovementW
        {
            get { return _numberMovementW; }
            set { _numberMovementW = value; }
        }
        private int NumberMovementB
        {
            get { return _numberMovementB; }
            set { _numberMovementB = value; }
        }
        private string NameW
        {
            get { return _nameW; }
            set { _nameW = value; }
        }
        private string NameB
        {
            get { return _nameB; }
            set { _nameB = value; }
        }
        private EController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        #endregion
        //constructor
        public EModel(EController controller)
        {
            this.Controller = controller;
            Pawns = new List<Pown>();
            NumberMovementB = 0;
            NumberMovementW = 0;
            Turn = true;
            TimeTimer = new Timer();
            TimeTimer.Tick += new EventHandler(TimeTimer_Tick);
            TimeTimer.Enabled = true;
            Pause = false;
        }

        // Action when new game
        public void NewGame(string nameW, string nameB)
        {
            // set data for new game
            NameB = nameB;
            NameW = nameW;
            TimeTimer.Enabled = true;
            Pawns = new List<Pown>();
            CreatePowns();
            Turn = true;
            Pause = false;
            TimeBlack = 0;
            TimeWhite = 0;
            NumberMovementB = 0;
            NumberMovementW = 0;

        }
        public void CreatePowns()
        {
            //Role 0=pawn/1=rook/2=knight/3=bishop/king=4/queen=5
            //creat pawn white
            for (int i = 1; i <= 8; i++)
            {
                Pown pawn = new Pown(0, "white", i, 7, "pawnWhite" + i.ToString());
                Pawns.Add(pawn);
            }
            //creat pawn black
            for (int i = 1; i <= 8; i++)
            {
                Pown pawn = new Pown(0, "black", i, 2, "pawnBlack" + i.ToString());
                Pawns.Add(pawn);
            }

            Pown rook1B = new Pown(1, "black", 1, 1, "rookBlack1"); Pawns.Add(rook1B);
            Pown rook2B = new Pown(1, "black", 8, 1, "rookBlack2"); Pawns.Add(rook2B);

            Pown knight1B = new Pown(2, "black", 2, 1, "knightBlack1"); Pawns.Add(knight1B);
            Pown knight2B = new Pown(2, "black", 7, 1, "knightBlack2"); Pawns.Add(knight2B);

            Pown bishop1B = new Pown(3, "black", 3, 1, "bishopBlack1"); Pawns.Add(bishop1B);
            Pown bishop2B = new Pown(3, "black", 6, 1, "bishopBlack2"); Pawns.Add(bishop2B);

            Pown kingB = new Pown(4, "black", 5, 1, "kingBlack"); Pawns.Add(kingB);
            Pown queenB = new Pown(5, "black", 4, 1, "queenBlack"); Pawns.Add(queenB);
            //change color
            Pown rook1W = new Pown(1, "white", 1, 8, "rookWhite1"); Pawns.Add(rook1W);
            Pown rook2W = new Pown(1, "white", 8, 8, "rookWhite2"); Pawns.Add(rook2W);

            Pown knight1W = new Pown(2, "white", 2, 8, "knightWhite1"); Pawns.Add(knight1W);
            Pown knight2W = new Pown(2, "white", 7, 8, "knightWhite2"); Pawns.Add(knight2W);

            Pown bishop1W = new Pown(3, "white", 3, 8, "bishopWhite1"); Pawns.Add(bishop1W);
            Pown bishop2W = new Pown(3, "white", 6, 8, "bishopWhite2"); Pawns.Add(bishop2W);

            Pown kingW = new Pown(4, "white", 5, 8, "kingWhite"); Pawns.Add(kingW);
            Pown queenW = new Pown(5, "white", 4, 8, "queenWhite"); Pawns.Add(queenW);
        }

        // Serializable/Deserializable
        public void SavePlay()
        {
            // creatlist because timer not serilizable
            List<object> saveItem = new List<object>();
            saveItem.Add(this.Pawns);
            saveItem.Add(this.Turn);
            saveItem.Add(this.TimeBlack);
            saveItem.Add(this.TimeWhite);
            saveItem.Add(this.NumberMovementB);
            saveItem.Add(this.NumberMovementW);
            saveItem.Add(this.NameB);
            saveItem.Add(this.NameW);
            FileStream file = File.Open(NameFileSave, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(file,saveItem );
            file.Close();

        }
        public void LoadPlay()
        {
            if (File.Exists(NameFileSave))
            {
                List<object> saveItem = new List<object>();
                FileStream fileOp = File.Open(NameFileSave, FileMode.Open);
                BinaryFormatter serializer = new BinaryFormatter();
                saveItem = (List<object>)serializer.Deserialize(fileOp);
                fileOp.Close();
                this.Pawns = (List<Pown>)saveItem[0];
                this.Turn = (bool)saveItem[1];
                this.TimeBlack = (int)saveItem[2];
                this.TimeWhite = (int)saveItem[3];
                this.NumberMovementB = (int)saveItem[4];
                this.NumberMovementW = (int)saveItem[5];
                this.NameB = (string)saveItem[6];
                this.NameW = (string)saveItem[7];
            }
        }

        // Prediction of move
        public List<List<int>> GetPossibleMovement(string name, int x, int y)
        {
            List<List<int>> mainList = new List<List<int>>();
            mainList = GetAllPosOfPawns();
            PositionsBag bag = new PositionsBag();
            string color = "";
            foreach (Pown item in this.Pawns)
            {
                if (name == item.Name)
                {
                    color = item.Color;
                    //Role 0=pawn/1=rook/2=knight/3=bishop/king=4/queen=5
                    switch (item.Role)
                    {
                        case 0:
                            bag = PossibleMovementOfPawn(item, x, y, mainList);
                            break;
                        case 1:
                            bag = PossibleMovementOfRook(x, y, mainList);
                            break;
                        case 2:
                            bag = PossibleMovementOfKnight(x, y);
                            break;
                        case 3:
                            bag = PossibleMovementOfBishop(x, y, mainList);
                            break;
                        case 4:
                            bag = PossibleMovementOfKing(x, y, item.Color);
                            break;
                        case 5:
                            bag = PossibleMovementOfQueen(x, y, mainList);
                            break;
                    }
                }
            }
            mainList = new List<List<int>>();

            mainList.Add(bag.XList); mainList.Add(bag.YList);
            mainList = RemoveFriendlyPos(mainList, color);
            return mainList;
        }
        #region Possible Movement
        private PositionsBag PossibleMovementOfPawn(Pown item, int x, int y, List<List<int>> mainList)
        {
            PositionsBag bag = new PositionsBag();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();
            bool moveDiagonalToken = true;
            bool twoMoveToken = true;

            for (int i = 0; i < mainList[0].Count; i++)
            {
                if (item.Color == "black")
                {
                    //check if a pown was in diagonal
                    if (mainList[0][i] == (item.PosX + 1) && (item.PosY + 1) == mainList[1][i])
                    {
                        xList.Add(x + 1); yList.Add(y + 1);
                    }
                    if (mainList[0][i] == (item.PosX - 1) && (item.PosY + 1) == mainList[1][i])
                    {
                        xList.Add(x - 1); yList.Add(y + 1);
                    }
                    //check if a pown was in front
                    if (mainList[1][i] == (item.PosY + 1) && mainList[0][i] == item.PosX)
                    {
                        moveDiagonalToken = false;
                    }
                    if (mainList[1][i] == (item.PosY + 1) && mainList[0][i] == item.PosX)
                    {
                        moveDiagonalToken = false;
                    }
                    if (mainList[1][i] == (item.PosY + 2) && mainList[0][i] == item.PosX)
                    {
                        twoMoveToken = false;
                    }
                    if (moveDiagonalToken == true && mainList[0].Count - 1 == i)
                    {
                        xList.Add(x); yList.Add(y + 1);                      
                    }
                    // for two case movement
                    if (item.CountMove == 0 && twoMoveToken == true)
                    {
                        xList.Add(x); yList.Add(y + 2);
                    }
                }
                else
                {
                    //check if a pown was in diagonal
                    if (mainList[0][i] == (item.PosX + 1) && (item.PosY - 1) == mainList[1][i])
                    {
                        xList.Add(x + 1); yList.Add(y - 1);
                    }
                    if (mainList[0][i] == (item.PosX - 1) && (item.PosY - 1) == mainList[1][i])
                    {
                        xList.Add(x - 1); yList.Add(y - 1);
                    }
                    //check if a pown was in front
                    if (mainList[1][i] == (item.PosY - 1) && mainList[0][i] == item.PosX)
                    {
                        moveDiagonalToken = false;
                    }
                    if (mainList[1][i] == (item.PosY - 2) && mainList[0][i] == item.PosX)
                    {
                        twoMoveToken = false;
                    }
                    if (moveDiagonalToken == true && mainList[0].Count - 1 == i)
                    {
                        xList.Add(x); yList.Add(y - 1);                        
                    }
                    // for two case movement
                    if (item.CountMove == 0 && twoMoveToken == true)
                    {
                        xList.Add(x); yList.Add(y - 2);
                    }
                }
            }

            bag.XList = xList;
            bag.YList = yList;
            return bag;
        }
        private PositionsBag PossibleMovementOfRook(int x, int y, List<List<int>> mainList)
        {
            PositionsBag bag = new PositionsBag();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();
            bool hitBoxToken = true;
            // Need separate calcule for each direction
            // West
            for (int i = x - 1; i > 0; i--)
            {
                if (hitBoxToken)
                {
                    xList.Add(i);
                    yList.Add(y);
                }
                for (int j = 0; j < mainList[0].Count; j++)
                {
                    if (y == mainList[1][j] && mainList[0][j] == i)
                    {
                        hitBoxToken = false;
                    }
                }
            }
            hitBoxToken = true;
            // East
            for (int i = x + 1; i <= 8; i++)
            {
                if (hitBoxToken)
                {
                    xList.Add(i);
                    yList.Add(y);
                }
                for (int j = 0; j < mainList[0].Count; j++)
                {
                    if (y == mainList[1][j] && mainList[0][j] == i)
                    {
                        hitBoxToken = false;
                    }
                }

            }
            hitBoxToken = true;
            // North
            for (int i = y + 1; i <= 8; i++)
            {
                if (hitBoxToken)
                {
                    xList.Add(x);
                    yList.Add(i);
                }
                for (int j = 0; j < mainList[0].Count; j++)
                {
                    if (x == mainList[0][j] && mainList[1][j] == i)
                    {
                        hitBoxToken = false;
                    }
                }

            }
            hitBoxToken = true;
            // South
            for (int i = y - 1; i > 0; i--)
            {
                if (hitBoxToken)
                {
                    xList.Add(x);
                    yList.Add(i);
                }
                for (int j = 0; j < mainList[0].Count; j++)
                {
                    if (x == mainList[0][j] && mainList[1][j] == i)
                    {
                        hitBoxToken = false;
                    }
                }
            }

            bag.XList = xList;
            bag.YList = yList;
            return bag;
        }
        private PositionsBag PossibleMovementOfKnight(int x, int y)
        {
            PositionsBag bag = new PositionsBag();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();
            xList.Add(x + 2); yList.Add(y - 1);
            xList.Add(x + 2); yList.Add(y + 1);
            xList.Add(x + 1); yList.Add(y - 2);
            xList.Add(x - 1); yList.Add(y - 2);
            xList.Add(x - 2); yList.Add(y - 1);
            xList.Add(x - 2); yList.Add(y + 1);
            xList.Add(x - 1); yList.Add(y + 2);
            xList.Add(x + 1); yList.Add(y + 2);
            bag.XList = xList;
            bag.YList = yList;
            return bag;
        }
        private PositionsBag PossibleMovementOfBishop(int x, int y, List<List<int>> mainList)
        {
            PositionsBag bag = new PositionsBag();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();

            bool northeastToken = true;
            bool southeastToken = true;
            bool northwestToken = true;
            bool southwestToken = true;
            for (int i = 1; i < 8; i++)
            {
                // Need separate calcule for each direction
                // Northeast
                if (northeastToken)
                {
                    xList.Add(x + i);
                    yList.Add(y - i);
                }
                // Southeast
                if (southeastToken)
                {
                    xList.Add(x + i);
                    yList.Add(y + i);
                }
                // Northwest
                if (northwestToken)
                {
                    xList.Add(x - i);
                    yList.Add(y - i);
                }
                // Southwest
                if (southwestToken)
                {
                    xList.Add(x - i);
                    yList.Add(y + i);
                }
                for (int j = 0; j < mainList[0].Count; j++)
                {
                    // Northeast
                    if (x + i == mainList[0][j] && mainList[1][j] == y - i)
                    {
                        northeastToken = false;
                    }
                    // Southeast
                    if (x + i == mainList[0][j] && mainList[1][j] == y + i)
                    {
                        southeastToken = false;
                    }
                    // Northwest
                    if (x - i == mainList[0][j] && mainList[1][j] == y - i)
                    {
                        northwestToken = false;
                    }
                    // Southwest
                    if (x - i == mainList[0][j] && mainList[1][j] == y + i)
                    {
                        southwestToken = false;
                    }
                }
            }
            bag.XList = xList;
            bag.YList = yList;
            return bag;
        }
        private PositionsBag PossibleMovementOfKing(int x, int y, string colorKing)
        {
            PositionsBag bag = new PositionsBag();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();
            // each movement with one scope
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                        xList.Add(i);
                        yList.Add(j);
                }
            }
            bag.XList = xList;
            bag.YList = yList;
            return bag;
        }
        private PositionsBag PossibleMovementOfQueen(int x, int y, List<List<int>> mainList)
        {
            PositionsBag bag = new PositionsBag();
            PositionsBag bagBishop = new PositionsBag();
            PositionsBag bagRook = new PositionsBag();
            // queen's movement is rook+bishop movement
            bagBishop = PossibleMovementOfBishop(x, y, mainList);
            bagRook = PossibleMovementOfRook(x, y, mainList);
            bag.AddTwoBags(bagBishop, bagRook);
            return bag;
        }
        #endregion
        private List<List<int>> RemoveFriendlyPos(List<List<int>> pos, string color)
        {
            for (int i = 0; i < pos[0].Count; i++)
            {
                foreach (Pown item2 in this.Pawns)
                {
                    if (pos[0][i] == item2.PosX && pos[1][i] == item2.PosY && color == item2.Color)
                    {
                        pos[1][i] = -1;
                        pos[0][i] = -1;
                    }
                }
            }
            return pos;
        }
        private List<List<int>> GetAllPosOfPawns()
        {
            List<List<int>> mainList = new List<List<int>>();
            List<int> xList = new List<int>();
            List<int> yList = new List<int>();
            foreach (Pown item in this.Pawns)
            {
                xList.Add(item.PosX);
                yList.Add(item.PosY);
            }
            mainList.Add(xList); mainList.Add(yList);
            return mainList;
        }

        public bool ChangePos(string name, int x, int y)
        {
            Boolean posChange = false;
            foreach (Pown item in Pawns)
            {
                if (name == item.Name)
                {
                    List<List<int>> mainList = GetPossibleMovement(name, item.PosX, item.PosY);
                    for (int i = 0; i < mainList[0].Count; i++)
                    {
                        if (mainList[0][i] == x && mainList[1][i] == y)
                        {
                            item.PosX = x;
                            item.PosY = y;
                            posChange = true;
                            item.CountMove += 1;
                            
                        }
                    }
                    // incrase movement of team
                    if (item.Color == "white")
                        NumberMovementW += 1;
                    else
                        NumberMovementB += 1;
                }
            }
            return posChange;
        }
        public void CheckDeath(int x, int y)
        {
            string color = "";
            if (Turn == false)
                color = "white";
            else
                color = "black";
            foreach (Pown item in Pawns)
            {
                if (item.Color == color && item.PosX == x && item.PosY == y)
                    item.Death = true;
                if (item.Color == color && item.PosX == x && item.PosY == y && item.Role == 4)
                {
                    MessageBox.Show("La partie est fini");
                    NewGame(NameW,NameB);
                }
            }
        }

        public string CheckTurn()
        {
            if (Turn == false)
            { return "Black"; }
            else
            { return "White"; }
        }
        public void ChangeTurn()
        {
            Turn = !Turn;
        }     

        //convert binary turn in string
        private string GetColorTurn()
        {
            string color = "";
            if (Turn == true)
                color = "white";
            else
                color = "black";
            return color;
        }

        public void Castling(string source, string second)
        {
            //set rook and king and verificate movement for castling
            Pown king = new Pown();
            Pown rook = new Pown();
            PositionsBag bag = new PositionsBag();
            Boolean castling = false;

            foreach (Pown item in Pawns)
            {
                if (item.Name == source)
                {
                    if (item.Role == 1)
                        rook = item;
                    else
                        king = item;
                }

                if (item.Name == second)
                {
                    if (item.Role == 1)
                        rook = item;
                    else
                        king = item;
                }
            }
            if (king.CountMove == 0 && rook.CountMove == 0)
            {
                bag = PossibleMovementOfRook(rook.PosX, rook.PosY, GetAllPosOfPawns());
                for (int i = 0; i < bag.XList.Count; i++)
                {
                    if (king.PosX == bag.XList[i] && king.PosY == bag.YList[i])
                    { castling = true; }
                }
            }

            if (castling == true)
            {
                if (rook.PosX == 1)
                {
                    king.PosX = king.PosX - 2;
                    rook.PosX = rook.PosX + 3;
                }
                else
                {
                    king.PosX = king.PosX + 2;
                    rook.PosX = rook.PosX - 2;
                }
            }
            ChangePos(king.Name, king.PosX, king.PosY);
            ChangePos(rook.Name, king.PosX, king.PosY);
        }
        public int GetNumberOfMove(string color)
        {
            int list = 0;
            if (color.Contains("white"))
                list = NumberMovementW;
            else
                list = NumberMovementB;
            return list;
        }

        public string GetTime(string color)
        {
            string time = "";
            if (color == "white")
                time = NormalizeTime(TimeWhite);
            else
                time = NormalizeTime(TimeBlack);
            return time;
        }
        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            if (Pause == false)
            {
                if (CheckTurn() == "White")
                    TimeWhite += 1;
                else
                    TimeBlack += 1;
            }

        }
        private string NormalizeTime(int timeInS)
        {
            // second to h:m:s for display
            string time = "";
            int h = 0;
            int m = 0;
            int s = timeInS;
            h = s / 3600;
            m = s / 60;
            s = s % 60;
            time = h.ToString() + ":" + m.ToString() + ":" + s.ToString();
            return time;
        }

        public string Checkmate()
        {
            string checkmate = "";
            List<List<int>> adverseMove = new List<List<int>>();
            foreach (Pown item in Pawns)
            {
                if (item.Role == 4)
                {
                    adverseMove = GetAllPossibleMovementAdverse(item.Color);
                    for (int i = 0; i < adverseMove[0].Count; i++)
                    {
                        if (adverseMove[0][i] == item.PosX && adverseMove[1][i] == item.PosY)
                        {
                            checkmate = item.Color;
                        }
                    }
                }
            }
            return checkmate;
        }
        public List<List<int>> GetAllPossibleMovementAdverse(string color)
        {
            List<List<int>> mainList = new List<List<int>>();
            mainList = GetAllPosOfPawns();
            PositionsBag bag = new PositionsBag();
            string colorPown = "";
            foreach (Pown item in this.Pawns)
            {
                if (color != item.Color)
                {
                    colorPown = item.Color;
                    //Role 0=pawn/1=rook/2=knight/3=bishop/king=4/queen=5
                    switch (item.Role)
                    {
                        case 0:
                            bag.AddBag(PossibleMovementOfPawn(item, item.PosX, item.PosY, mainList));
                            break;
                        case 1:
                            bag.AddBag(PossibleMovementOfRook(item.PosX, item.PosY, mainList));
                            break;
                        case 2:
                            bag.AddBag(PossibleMovementOfKnight(item.PosX, item.PosY));
                            break;
                        case 3:
                            bag.AddBag(PossibleMovementOfBishop(item.PosX, item.PosY, mainList));
                            break;
                        case 4:
                            // make nothing else overflow
                            break;
                        case 5:
                            bag.AddBag(PossibleMovementOfQueen(item.PosX, item.PosY, mainList));
                            break;
                    }
                }
            }
            mainList = new List<List<int>>();

            mainList.Add(bag.XList); mainList.Add(bag.YList);
            return mainList;
        }

    }


}
