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
            using (volunteersEntities entities = new volunteersEntities())
            {
                Dictionary<user, UserHoursState> usersState = UserHoursState.createListOfUsersHourState(currGroup,groupEvents);
                if (usersState == null) return null;
                //create array with event and num of volunteers that agree to volunteer
                Dictionary<@event, List<user>> eventWithListOfUsers = createEventWithListOfUsersDictionary(groupEvents);
                //average hours
                decimal average = groupEvents.Sum(elem => elem.NumOfHouers).GetValueOrDefault();
                if (average > 0)
                    average = average / currGroup.user_to_group.Count();
                inlayToUsersWithLessThanAverage(eventWithListOfUsers,usersState,average);
                foreach (KeyValuePair<@event, List<user>> entry in eventWithListOfUsers)
                {
                    //state to current event
                    Dictionary<user, UserHoursState> usersStateToCurrEvent =
                            usersState.ToList().Where(us => entry.Value.Contains(us.Key)).
                            OrderBy(us => us.Value.Left).
                            ThenByDescending(us => us.Value.Taken).
                            ToDictionary(pair => pair.Key, pair => pair.Value);
                    if (usersStateToCurrEvent.Count == 0)
                        break;
                    @event eventToSave = entities.events.FirstOrDefault(element => element.id == entry.Key.id);
                    user userWithMinHours = usersStateToCurrEvent.FirstOrDefault(us => !us.Value.IsMoreAvg).Key;
                    if (userWithMinHours == null)
                    {
                        userWithMinHours = usersStateToCurrEvent.ToList().FirstOrDefault().Key;
                    }
                    if (eventToSave == null)
                        return null;
                    eventToSave.OwnerId = userWithMinHours.id;
                    decimal currentHours = eventToSave.NumOfHouers.GetValueOrDefault();
                    usersState[userWithMinHours].Taken += currentHours;
                    if (usersState[userWithMinHours].Taken > average)
                        usersState[userWithMinHours].IsMoreAvg = true;
                    foreach (user item in entry.Value)
                    {
                        usersState[item].Left -= currentHours;
                    }
                }
                entities.SaveChanges();
            }
            using (volunteersEntities entities = new volunteersEntities())
            {
                return Convert.EventConverter.ConvertToListOfEventDTO(entities.events.ToList());
            }
        }
        private static Dictionary<@event, List<user>> createEventWithListOfUsersDictionary(List<@event> groupEvents)
        {
            Dictionary<@event, List<user>> eventWitListOfUsers = new Dictionary<@event, List<user>>();
            groupEvents.ForEach(ge =>
            {
                eventWitListOfUsers.Add(ge, ge.event_to_user.Select(etu => etu.user).ToList());
            });
            return eventWitListOfUsers.OrderBy(x => x.Value.Count).
                                ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        private static void inlayToUsersWithLessThanAverage(Dictionary<@event, List<user>> eventWithListOfUsers,Dictionary<user, UserHoursState> usersState, decimal avg)
        {
            using (volunteersEntities entities = new volunteersEntities())
            {
                Dictionary<user, UserHoursState> usersStateWithLess =
                    usersState.Where(us => us.Value.Left < avg).
                               OrderBy(us => us.Value.Left).
                               ToDictionary(pair => pair.Key, pair => pair.Value);
                if (usersStateWithLess.Count == 0) return;
                eventWithListOfUsers.ToList().ForEach(eventWithUsers =>
                {
                    decimal numOfHouers = eventWithUsers.Key.NumOfHouers.GetValueOrDefault();
                    @event eventToSave = entities.events.Find(eventWithUsers.Key.id);
                    //item.Value - the first user that exist in list of users of this event
                    List<user> matchUsers =
                    usersStateWithLess.ToList().
                                       Where(userWithHours => eventWithUsers.Value.Select(val => val.id).ToList().Contains(userWithHours.Key.id))
                                      .Select(u => u.Key).ToList();
                    if (matchUsers.Count > 0)
                    {
                        user choosenUser = matchUsers.First();
                        eventToSave.OwnerId = choosenUser.id;
                        eventWithListOfUsers.Remove(eventWithUsers.Key);
                        usersState[choosenUser].Taken += numOfHouers;
                        matchUsers.ForEach(mu =>
                        {
                            usersState[mu].Left -= numOfHouers;
                        });
                        usersStateWithLess = SortByLeft(usersStateWithLess);
                    }
                    else
                    {
                        eventToSave.OwnerId = null;
                    }
                });
                entities.SaveChanges();
            }
        }
        private static Dictionary<user, UserHoursState> SortByLeft(Dictionary<user, UserHoursState> usersState)
        {
            return usersState.OrderBy(us => us.Value.Left).
                              ThenByDescending(us => us.Value.Taken).
                              ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
