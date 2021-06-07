using System;

namespace CarsAndTrains.Classes.Nodes
{
    class TrainTriggerNode : Node
    {
        #region Fields
        private Node turnpike;
        #endregion

        #region Constructors
        public TrainTriggerNode()
        {

        }
        public TrainTriggerNode(Node turnpike)
        {

        }
        #endregion

        #region Methods
        public void TriggerTurnpike()
        {
            throw new NotImplementedException();
            //funcja uruchomi się, gdy pociąg wjedzie na nodea
            //Ma zmienić światła na przeciwne i zablokować  node'a dla samochodów przed torami
        }
        #endregion
    }
}
