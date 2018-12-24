using System.Collections.Generic;

namespace Echec_Johner
{
    class EController
    {
        #region var

        EModel _model;
        EView _view;

        #endregion
        //constructor
        public EController(EView view)
        {
            _view = view;
            _model = new EModel(this);
        }

        // refresh when need information of model
        public void RefreshView()
        {
            foreach (Pown item in _model.Pawns)
            {
                if (item.Death == false)
                    _view.InvestmentPown(item.PosX, item.PosY, item.Name);
                else
                {
                    _view.InvestementDeadPown(item.Name);
                    item.PosX = -1;
                    item.PosY = -1;
                }
            }
        }

        public void CreatePowns()
        {
            _model.CreatePowns();
        }      
        public List<List<int>> GetPossibleMovement(string name, int x, int y)
        {
            return _model.GetPossibleMovement(name, x, y);
        }
        public bool ChangePos(string name, int x, int y)
        {
            return _model.ChangePos(name, x, y);
        }
        public void NewGame(string nameW, string nameB)
        {
            _model.NewGame(nameW, nameB);
        }
        public string CheckTurn()
        {
            return _model.CheckTurn();
        }
        public void ChangeTurn()
        {
            _model.ChangeTurn();
        }
        public void CheckDeath(int x, int y)
        {
            _model.CheckDeath(x, y);
        }
        public void Castling(string source, string second)
        {
            _model.Castling(source, second);
        }
        public string GetNumberOfMove(string color)
        {
            int list = 0;
            list = _model.GetNumberOfMove(color);
            // convert int to string for display
            return list.ToString();
        }
        public string GetTime(string color)
        {
            string time = "";
            time = _model.GetTime(color);
            return time;
        }
        public string Checkmate()
        {
            string checkmate = "";
            checkmate = _model.Checkmate();
            return checkmate;
        }
        public void Save()
        {
            _model.SavePlay();
        }
        public void Load()
        {
            _model.LoadPlay();
        }
        public void ChangePause()
        {
             _model.Pause = !_model.Pause;
        }
        public bool GetPause()
        {
            return _model.Pause;
        }
    }
}
