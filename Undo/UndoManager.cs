using System.Collections.Generic;
using NeoDraw.UI;
using static Terraria.ModLoader.ModContent;

namespace NeoDraw.Undo {
    
    public class UndoManager {

        #region Variables

        #region Private Functions

        private readonly int _maxHistoryCount;
        private readonly LinkedList<UndoStep> _history;
        private readonly LinkedList<UndoStep> _redo;

        #endregion

        #region Public Variables

        public int HistoryCount { get { return _history.Count; } }
        public int RedoCount    { get { return _redo.Count;    } }

        #endregion

        #endregion

        public UndoManager(int maxHistoryCount = 10) {

            _history = new LinkedList<UndoStep>();
            _redo    = new LinkedList<UndoStep>();
            _maxHistoryCount = maxHistoryCount;

        }

        #region Public Functions

        public void ClearHistory() => _history.Clear();

        public void ClearRedo() => _redo.Clear();

        public void Redo()  {

            UndoStep stepToRedo = RedoPop();

            if (stepToRedo == null)
                return;

            DrawInterface.SetStatusBarTempMessage("Redo", 50);

            UndoPush(stepToRedo, true);
            stepToRedo.Undo();

        }

        public UndoStep RedoPop() {

            if (_redo == null || _redo.Count == 0)
                return null;

            UndoStep undoStep = _redo.First.Value;
            _redo.RemoveFirst();

            return undoStep;

        }

        public void RedoPush(UndoStep undoStep) {
            
            if (undoStep == null || undoStep.Count == 0)
                return;

            _redo.AddFirst(undoStep);

            while (_redo.Count > _maxHistoryCount)
                _redo.RemoveLast();

        }

        public void Reset() {
            
            ClearHistory();
            ClearRedo();

        }

        public void Undo() {
            
            UndoStep stepToUndo = UndoPop();

            if (stepToUndo == null)
                return;

            string statusBarMessage = "Undo";

            if (GetInstance<NeoConfigServer>().DebugMode)
                statusBarMessage += $" (Count: {stepToUndo.Count})";

            DrawInterface.SetStatusBarTempMessage(statusBarMessage, 50);

            RedoPush(stepToUndo);
            stepToUndo.Undo();

        }

        public UndoStep UndoPop() {

            if (_history == null || _history.Count == 0)
                return null;

            UndoStep undoStep = _history.First.Value;
            _history.RemoveFirst();

            return undoStep;

        }

        public void UndoPush(UndoStep undoStep, bool redoing = false) {

            if (undoStep == null || undoStep.Count == 0)
                return;

            _history.AddFirst(undoStep);

            while (_history.Count > _maxHistoryCount)
                _history.RemoveLast();

            if (!redoing)
                ClearRedo();

        }

        #endregion

    }

}
