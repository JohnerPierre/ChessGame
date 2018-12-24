 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Echec_Johner
{
    [System.Serializable]
    class Pown
    {
        #region var

        int _role;   // 0 for pawn 1 for rook 2 for kniht 3 for bishop 4 for queen 5 for king
        string _color;
        bool _death; // 0 for dead 1 for alive
        int _posX;
        int _posY;
        string _name;
        int _picture;
        int _countMove;

        #endregion
        #region Getteur/Setter
        public int CountMove
        {
            get { return _countMove; }
            set { _countMove = value; }
        }
        public int Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public bool Death
        {
            get { return _death; }
            set { _death = value; }
        }
        public int PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }
        public int PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        #endregion
        public Pown(int rolePown,string colorPown,int posXPown,int posYPown,string namePown)
        {
            this.Role = rolePown;
            this.Color = colorPown;
            this.PosX = posXPown;
            this.PosY = posYPown;
            this.Name = namePown;
            this.Picture = 0;
            this.Death = false;
            this.CountMove = 0;
        }
        public Pown()
        {
            this.Role = 0;
            this.Color = "";
            this.PosX = 5;
            this.PosY = 5;
            this.Name = "";
            this.Picture = 0;
            this.Death = false;
            this.CountMove = 0;
        }
    }
}
