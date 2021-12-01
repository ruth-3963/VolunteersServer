using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Inlay
{
    public class InlayEvents
    {
        internal static List<EventDTO> Calc(group currGroup, List<event_to_user> eventsToUsers, List<@event> groupEvents)
        {
            Dictionary<user, UserHoursState> usersState = UserHoursState.createListOfUsersHourState(currGroup);
            if(usersState == null) return null;
            return null;
        }

      
    }
}
