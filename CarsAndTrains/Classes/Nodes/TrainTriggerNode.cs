using System;
using System.Windows;

namespace CarsAndTrains.Classes.Nodes
{
    /// <summary>
    /// Variant of a normal Node, triggering a Turnpike once a vehicle interacts with it
    /// </summary>
    class TrainTriggerNode : Node
    {
        #region Constructors

        public TrainTriggerNode(Point position)
        {
            CanGoTo = true; 
            this.Position = position;
        }

        #endregion

        #region Methods
        public void TriggerTurnpike() => PublicAvaliableReferences.TriggerTurnPike();
        #endregion
    }
}
