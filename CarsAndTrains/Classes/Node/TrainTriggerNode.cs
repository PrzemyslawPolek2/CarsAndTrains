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
        private Node turnpike;

        public TrainTriggerNode()
        {

        }

        public void TriggerTurnpike()
        {
            //funcja uruchomi się, gdy pociąg wjedzie na nodea
            //Ma zmienić światła na przeciwne i zablokować  node'a dla samochodów przed torami
        }

    }
}
