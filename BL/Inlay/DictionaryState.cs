using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL.Inlay
{
    public class DictionaryState
    {
        static volunteersEntities db = new volunteersEntities();

        private Dictionary<user, UserHoursState> dictionaryState = new Dictionary<user, UserHoursState>();

        public Dictionary<user, UserHoursState> createListOfUsersHourState(group currGroup)
        {
            Dictionary<user, UserHoursState> state = new Dictionary<user, UserHoursState>();
            List<user> users = currGroup.user_to_group.Select(ug => ug.user).ToList();
            users.ForEach(u =>
            {
                List<@event> usersEvents = db.event_to_user.Where
                (etu => etu.groupId == currGroup.id && etu.userId == u.id).Select(etu => etu.@event).ToList();
                if (usersEvents.Count > 0)
                {
                    decimal possible = usersEvents.Sum(ue => ue.NumOfHouers).Value;
                    state.Add(u, new UserHoursState(possible, 0, 0));
                }
            });
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
