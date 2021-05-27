using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains
{
    class TrainTriggerNode : Node
    {
        private Node turnpike { get; set; }

        public TrainTriggerNode()
        {

        }

        public void triggerTurnpike()
        {
            //funcja uruchomi się, gdy pociąg wjedzie na nodea
            //Ma zmienić światła na przeciwne i zablokować  node'a dla samochodów przed torami
        }

    }
}
