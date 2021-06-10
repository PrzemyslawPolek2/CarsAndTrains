using System;
using System.Windows;

namespace CarsAndTrains.Classes.Nodes
{
    class TrainTriggerNode : Node
    {
        #region Constructors

        public TrainTriggerNode(Point position) : base(position)
        {
            
        }

        #endregion

        #region Methods
        public void TriggerTurnpike() => PublicAvaliableReferences.TriggerTurnPike();
        #endregion
    }
}
