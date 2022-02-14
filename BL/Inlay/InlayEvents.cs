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
                //Dictionary that hold user(key) and value of status hours during the algorithem
                Dictionary<user, UserHoursState> usersState = UserHoursState.createListOfUsersHourState(currGroup,groupEvents);
                if (usersState == null) return null;
                //create array with event and num of volunteers that agree to volunteer
                Dictionary<@event, List<user>> eventWithListOfUsers = createEventWithListOfUsersDictionary(groupEvents);
                //average hours
                decimal average = groupEvents.Sum(elem => elem.NumOfHouers).GetValueOrDefault();
                if (average > 0)
                    average = average / usersState.Count();
                else { 
                    return null; 
                }
                inlayToUsersWithLessThanAverage(eventWithListOfUsers,usersState,average);
                foreach (KeyValuePair<@event, List<user>> entry in eventWithListOfUsers)
                {
                    @event eventToSave = entities.events.FirstOrDefault(element => element.id == entry.Key.id);
                    if (entry.Value.Count == 0 || eventToSave == null) continue;
                    //case that only one user agree to volunteer
                    if (entry.Value.Count == 1)
                    {
                        eventToSave.OwnerId = entry.Value[0].id;
                        //inlay choosen user in event
                        UserHoursState.inlayUser(entry.Value[0], eventToSave, entry.Value, usersState, average);
                        continue;
                    }
                    //state to current event
                    Dictionary<user, UserHoursState> usersStateToCurrEvent =
                        //get all match users from usersState
                            usersState.ToList().Where(us => entry.Value.Contains(us.Key)).
                            OrderBy(us => us.Value.Left).
                            ThenBy(us => us.Value.Taken).
                            ToDictionary(pair => pair.Key, pair => pair.Value);
          
                    user selectedUser = usersStateToCurrEvent.FirstOrDefault(us => !us.Value.IsMoreAvg).Key;
                    if (selectedUser == null)
                    {
                        selectedUser = usersStateToCurrEvent.ToList().FirstOrDefault().Key;
                    }
                    eventToSave.OwnerId = selectedUser.id;
                    //inlay the choosen user in event
                    UserHoursState.inlayUser(selectedUser, eventToSave, entry.Value, usersState, average);
                }
                entities.SaveChanges();
            }
            using (volunteersEntities entities = new volunteersEntities())
            {
                return Convert.EventConverter.ConvertToListOfEventDTO(entities.events.Where(e => e.GroupId == currGroup.id).ToList());
            }
        }
        private static Dictionary<@event, List<user>> createEventWithListOfUsersDictionary(List<@event> groupEvents)
        {
            Dictionary<@event, List<user>> eventWitListOfUsers = new Dictionary<@event, List<user>>();
            groupEvents.ForEach(ge =>
            {
                eventWitListOfUsers.Add(ge, ge.event_to_user.Select(etu => etu.user)?.ToList());
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
