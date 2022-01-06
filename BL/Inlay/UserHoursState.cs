using DAL;
using System.Collections.Generic;
using System.Linq;
namespace BL.Inlay
{

    public class UserHoursState
    {
        public decimal Taken { get; set; }
        public decimal Left { get; set; }
        public bool IsMoreAvg { get; set; }
        public UserHoursState(decimal left, decimal taken, bool isMoreAvg)
        {
            Left = left;
            Taken = taken;
            IsMoreAvg = isMoreAvg;
        }

        public static Dictionary<user, UserHoursState> createListOfUsersHourState(group currGroup, List<@event> groupEvents)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                Dictionary<user, UserHoursState> state = new Dictionary<user, UserHoursState>();
                List<user> users = currGroup.user_to_group.Select(ug => ug.user).ToList();

                users.ForEach(u =>
                {
                    List<@event> usersEvents = db.event_to_user
                    .Where(etu => etu.groupId == currGroup.id && etu.userId == u.id && groupEvents.Select(e => e.id).Contains(etu.eventId))
                    .Select(etu => etu.@event).ToList();
                    if (usersEvents.Count > 0)
                    {
                        decimal left = usersEvents.Sum(ue => ue.NumOfHouers).Value;
                        state.Add(u, new UserHoursState(left, 0, false));
                    }
                });
                state = state.OrderBy(val => val.Value.Left).ThenByDescending(val => val.Value.Taken).ToDictionary(pair => pair.Key, pair => pair.Value);
                return state;
            }
        }
        public static Dictionary<user, UserHoursState> SortUserState(Dictionary<user, UserHoursState> state)
        {
            Dictionary<user, UserHoursState> newState = new Dictionary<user, UserHoursState>();
            newState = (Dictionary<user, UserHoursState>)state.OrderBy(s => s.Value);
            return newState;
        }

    }
}


