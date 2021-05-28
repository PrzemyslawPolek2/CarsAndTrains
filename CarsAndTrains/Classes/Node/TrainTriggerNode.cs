using CarsAndTrains.Classes.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes.Node
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
