using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BL.Inlay
{

    public class UserHoursState : IComparable
    {
        static volunteersEntities db = new volunteersEntities();
        public decimal Possible { get; set; }
        public decimal Taken { get; set; }
        public decimal Left { get; set; }
        public UserHoursState(decimal possible, decimal taken, decimal left)
        {
            Possible = possible;
            Taken = taken;
            Left = left;
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
                    decimal possible = usersEvents.Sum(ue => ue.NumOfHouers).Value;
                    state.Add(u, new UserHoursState(possible, 0, 0));
                }
            });
            state = state.OrderBy(val => val.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return state;
        }
        public static Dictionary<user, UserHoursState> SortUserState(Dictionary<user, UserHoursState> state)
        {
            Dictionary<user, UserHoursState> newState = new Dictionary<user, UserHoursState>();
            newState = (Dictionary<user, UserHoursState>)state.OrderBy(s => s.Value);
            return newState;
        }

        //public int Compare(object x, object y)
        //{
        //    if (x is UserHoursState && y is UserHoursState)
        //        return (x as UserHoursState).Possible.CompareTo((y as UserHoursState).Possible);
        //    return 0;
        //}
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            return Possible > (obj as UserHoursState).Possible;
        }
        //public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
        //{
        //    var dictionary = new Dictionary<TValue, TKey>();
        //    foreach (var entry in source)
        //    {
        //        if (!dictionary.ContainsKey(entry.Value))
        //            dictionary.Add(entry.Value, entry.Key);
        //    }
        //    return dictionary;
        //}
        int IComparable.CompareTo(object obj)
        {
            UserHoursState c = (UserHoursState)obj;
            if (c.Possible < Possible)
                return 1;

            if (c.Possible > Possible)
                return -1;

            else
                return 0;
        }
    }
}


