using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BL.Inlay
{

    public class UserHoursState
    {
        static volunteersEntities db = new volunteersEntities();
        public decimal Taken { get; set; }
        public decimal Left { get; set; }
        public bool IsMoreAvg { get; set; }
        public UserHoursState(decimal left,decimal taken, bool isMoreAvg)
        {
            Left = left;
            Taken = taken;
            IsMoreAvg = isMoreAvg;
        }

        public static Dictionary<user, UserHoursState> createListOfUsersHourState(group currGroup)
        {
            Dictionary<user, UserHoursState> state = new Dictionary<user, UserHoursState>();
            List<user> users = currGroup.user_to_group.Select(ug => ug.user).ToList();

            users.ForEach(u =>
            {
                List<@event> usersEvents = db.event_to_user.Where
                (etu => etu.groupId == currGroup.id && etu.userId == u.id).Select(etu => etu.@event).ToList();
                if (usersEvents.Count > 0)
                {
                    decimal left = usersEvents.Sum(ue => ue.NumOfHouers).Value;
                    state.Add(u, new UserHoursState(left,0, false));
                }
            });
            state = state.OrderBy(val => val.Value.Left).ThenByDescending(val => val.Value.Taken).ToDictionary(pair => pair.Key, pair => pair.Value);
            return state;
        }
        public static Dictionary<user, UserHoursState> SortUserState(Dictionary<user, UserHoursState> state)
        {
            Dictionary<user, UserHoursState> newState = new Dictionary<user, UserHoursState>();
            newState = (Dictionary<user, UserHoursState>)state.OrderBy(s => s.Value);
            return newState;
        }
      
    }
}


