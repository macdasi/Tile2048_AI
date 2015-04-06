using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.BlackJack;
using WebApplication1.Models.JacksOrBetter;

namespace WebApplication1.Logic
{
    public class JacksOrBetterLogic
    {
        private Actions[] _actions;
        private List<Card> _sortedUp = new List<Card>();
        private Dictionary<Card, int> _cardPositions = new Dictionary<Card, int>();
        private int[] _selectors = new int[5]{1,1,1,1,1};

        private Card FromString(string cardStr)
        {

            RANK rank = (RANK)Enum.Parse(typeof(RANK), "R" + cardStr.Split('_')[1]);
            SUIT suit = (SUIT)Enum.Parse(typeof(SUIT), cardStr.Split('_')[0]);
            return new Card(rank, suit);
        }

        public JacksOrBetterLogic(Actions[] actions, string[] player)
        {
            _actions = actions;

            if (_actions.Count() == 1 && _actions[0] == Actions.BET)
            {
                return;
            }

            for (int i = 0; i < player.Length; i++)
            {
                _cardPositions.Add(FromString(player[i]), i);
            }

            var l = _cardPositions.OrderBy(key => key.Key.rank);
            _sortedUp = l.Select(x => x.Key).ToList();
        }

        public JoBmoves GetMove()
        {
           
            if (_actions.Count() == 1 && _actions[0] == Actions.BET)
            {
                return new JoBmoves() { move = _actions[0], hold = null };
            }
            else if (_actions.Count() == 1 && _actions[0] == Actions.STAND)
            {
                
                JoBmoves move = new JoBmoves() { move = Actions.STAND, hold = _selectors };

                if (isRoyalFlush())
                {
                    move.hold = _selectors;
                    return move;
                }
                if (isFlush())
                {
                    move.hold = _selectors;
                    return move;
                }
                if (isFourOfAKind())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (Is4ToRoyalFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (isFullHouse())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (isFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (isThreeOfAKind())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (isStraight())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (is4ToStraightFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (isTwoPair())
                {
                    move.hold = _selectors;
                    return move;
                }
                if (isJacksOrBetter())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (Is3ToRoyalFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (Is4ToFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (is4ToStraight())
                {
                    move.hold = _selectors;
                    return move;
                }
                if (isLowPair())
                {
                    move.hold = _selectors;
                    return move;
                }

                if (is4_to_outside_straight_0To2_high_cards())
                {
                     move.hold = _selectors;
                    return move;
                }
                
                if (is1JackOrBetter())
                {
                    move.hold = _selectors;
                    return move;
                }
                if (is3ToStraightFlush())
                {
                    move.hold = _selectors;
                    return move;
                }

                _selectors = new int[5] { 0, 0, 0, 0, 0 };
                move.hold = _selectors;
                return move;


                //Suited QJ (0.6004)B
                //4 to an inside straight, 4 high cards (0.5957)
                //Suited KQ or KJ (0.5821)
                //Suited AK, AQ, or AJ (0.5678)
                //4 to an inside straight, 3 high cards (0.5319)
                //3 to a straight flush (type 2) (0.5227 to 0.5097)C
                //Unsuited JQK (0.5005)
                //Unsuited JQ (0.4980)
                //Suited TJ (0.4968) D
                //2 unsuited high cards king highest (0.4862)
                //Suited TQ (0.4825) E
                //2 unsuited high cards ace highest (0.4743)
                //J only (0.4713)
                //Suited TK (0.4682) F
                //Q only (0.4681)
                //K only (0.4649)
                //A only (0.4640)
                //3 to a straight flush (type 3) (0.4431)
                //Garbage, discard everything (0.3597)



            }
            throw new Exception("Logic error");
        }

        private bool is3ToStraightFlush()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank - 1
                && _sortedUp[1].rank == _sortedUp[2].rank - 1
                && SameSuit(_sortedUp[0], _sortedUp[1], _sortedUp[2])
                )
            {
                HoldBySorted(1, 1, 1, 0, 0);
                return true;
            }

            if (_sortedUp[1].rank == _sortedUp[2].rank - 1
                && _sortedUp[2].rank == _sortedUp[3].rank - 1
                && SameSuit(_sortedUp[1], _sortedUp[2], _sortedUp[3])
                )
            {
                HoldBySorted(0, 1, 1, 1, 0);
                return true;
            }

            if (_sortedUp[2].rank == _sortedUp[3].rank - 1
                && _sortedUp[3].rank == _sortedUp[4].rank - 1
                && SameSuit(_sortedUp[2], _sortedUp[3], _sortedUp[4])
                )
            {
                HoldBySorted(0, 0, 1, 1, 1);
                return true;
            }


            return false;
        }

        

        

        private bool HasHighCards(params Card[] list)
        {
            foreach (var item in list)
            {
                if (item.isJacksOrBetter())
                {
                    return true;
                }
            }
            return false;
        }

        private bool is4_to_outside_straight_0To2_high_cards() 
        {
            if (// x-9-T-J-Q or x-8-9-T-J
                _sortedUp[1].rank == _sortedUp[2].rank - 1
               && _sortedUp[2].rank == _sortedUp[3].rank - 1
               && _sortedUp[3].rank == _sortedUp[4].rank - 1
               &&HasHighCards(_sortedUp[3],_sortedUp[4])
                )
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        private bool is1JackOrBetter()
        {
            int[] setToHold = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < _sortedUp.Count(); i++)
            {
                if (_sortedUp[i].isJacksOrBetter())
                {
                    setToHold[i] = 1;
                }
            }
            if (setToHold.Contains(1)) {
                HoldBySorted(setToHold[0], setToHold[1], setToHold[2], setToHold[3], setToHold[4]);
                return true;
            }
            
            
            
            
            return false;
        }

        private bool isLowPair()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank)
            {
                HoldBySorted(1, 1, 0, 0, 0);
                return true;
            }
            if (_sortedUp[1].rank == _sortedUp[2].rank )
            {
                HoldBySorted(0, 1, 1, 0, 0);
                return true;
            }
            if (_sortedUp[2].rank == _sortedUp[3].rank)
            {
                HoldBySorted(0, 0, 1, 1, 0);
                return true;
            }
            if (_sortedUp[3].rank == _sortedUp[4].rank)
            {
                HoldBySorted(0, 0, 0, 1, 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// //Unsuited TJQK(0.8723)
        /// </summary>
        /// <returns></returns>
        private bool is4ToStraight()
        {
            if (
                _sortedUp[1].rank == RANK.RT
                &&_sortedUp[1].rank == _sortedUp[2].rank - 1
                && _sortedUp[2].rank == _sortedUp[3].rank - 1
                && _sortedUp[3].rank == _sortedUp[4].rank - 1
                
                )
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        private bool Is4ToFlush()
        {
            if (SameSuit(_sortedUp[0], _sortedUp[1], _sortedUp[2], _sortedUp[3]))
            {
                HoldBySorted(1, 1, 1, 1, 0);
                return true;
            }
            if (SameSuit(_sortedUp[0], _sortedUp[1], _sortedUp[2], _sortedUp[4]))
            {
                HoldBySorted(1, 1, 1, 0, 1);
                return true;
            }
            if (SameSuit(_sortedUp[0], _sortedUp[1], _sortedUp[3], _sortedUp[4]))
            {
                HoldBySorted(1, 1, 0, 1, 1);
                return true;
            }
            if (SameSuit(_sortedUp[0], _sortedUp[2], _sortedUp[3], _sortedUp[4]))
            {
                HoldBySorted(1, 0, 1, 1, 1);
                return true;
            }
            if (SameSuit(_sortedUp[4], _sortedUp[1], _sortedUp[2], _sortedUp[3]))
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        private bool is4ToStraightFlush()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank - 1
                && _sortedUp[1].rank == _sortedUp[2].rank - 1
                && _sortedUp[2].rank == _sortedUp[3].rank - 1
                && SameSuit(_sortedUp[0], _sortedUp[1], _sortedUp[2], _sortedUp[3])
                )
            {
                HoldBySorted(1, 1, 1, 1, 0);
                return true;
            }
            if (_sortedUp[1].rank == _sortedUp[2].rank - 1
                && _sortedUp[2].rank == _sortedUp[3].rank - 1
                && _sortedUp[3].rank == _sortedUp[4].rank - 1
                && SameSuit(_sortedUp[1], _sortedUp[2], _sortedUp[3], _sortedUp[4])
            )
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        private void HoldBySorted(params int[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                _selectors[_cardPositions[_sortedUp[i]]] = list[i];
            }
        }

        private static bool SameSuit(params Card[] list)
        {
            SUIT s = list[0].suit;
            for (int i = 0; i < list.Length; i++)
            {
                if (s != list[i].suit)
                {
                    return false;
                }
            }
            return true;
        }

        private bool Is4ToRoyalFlush()
        {
            if (_sortedUp[0].rank == RANK.RA
                && _sortedUp[2].rank == RANK.RJ
                && _sortedUp[3].rank == RANK.RQ
                && _sortedUp[4].rank == RANK.RK
                && SameSuit(_sortedUp[0], _sortedUp[2], _sortedUp[3], _sortedUp[4]))
            {
                //A,x,J,Q,K
                HoldBySorted(1, 0, 1, 1, 1);
                return true;
            }
            if (_sortedUp[1].rank == RANK.RT
                && _sortedUp[2].rank == RANK.RJ
                && _sortedUp[3].rank == RANK.RQ
                && _sortedUp[4].rank == RANK.RK
                && (SameSuit(_sortedUp[0], _sortedUp[2], _sortedUp[3], _sortedUp[4]))
            )
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        private bool Is3ToRoyalFlush()
        {
            if (_sortedUp[0].rank == RANK.RA
                && _sortedUp[3].rank == RANK.RJ
                && _sortedUp[4].rank == RANK.RQ
                && SameSuit(_sortedUp[0], _sortedUp[3], _sortedUp[4]))
            {
                //A,x , x ,J,Q
                HoldBySorted(1, 0, 0, 1, 1);
                return true;
            }
            if (_sortedUp[0].rank == RANK.RA
                && _sortedUp[3].rank == RANK.RQ
                && _sortedUp[4].rank == RANK.RK
                && SameSuit(_sortedUp[0], _sortedUp[3], _sortedUp[4]))
            {
                //A,x , x ,Q,K
                HoldBySorted(1, 0, 0, 1, 1);
                return true;
            }
            if (_sortedUp[2].rank == RANK.RJ
                && _sortedUp[3].rank == RANK.RQ
                && _sortedUp[4].rank == RANK.RK
                && SameSuit(_sortedUp[2], _sortedUp[3], _sortedUp[4]))
            {
                //x,x , J ,Q,K
                HoldBySorted(0, 0, 1, 1, 1);
                return true;
            }
            return false;
        }
        private bool isFlush()
        {
            if (_sortedUp[0].suit == _sortedUp[1].suit &&
                _sortedUp[1].suit == _sortedUp[2].suit &&
                _sortedUp[2].suit == _sortedUp[3].suit &&
                _sortedUp[3].suit == _sortedUp[4].suit)
                return true;
            return false;
        }

        // make sure the rank differs by one
        // we can do this since the Hand is 
        // sorted by this point
        private bool isStraight()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank - 1 &&
                _sortedUp[1].rank == _sortedUp[2].rank - 1 &&
                _sortedUp[2].rank == _sortedUp[3].rank - 1 &&
                _sortedUp[3].rank == _sortedUp[4].rank - 1)
                return true;
            // special case cause ace ranks lower
            // than 10 or higher
            if (_sortedUp[0].rank == RANK.RA &&
                  _sortedUp[1].rank == RANK.RT &&
                  _sortedUp[2].rank == RANK.RJ &&
                  _sortedUp[3].rank == RANK.RQ &&
                  _sortedUp[4].rank == RANK.RK)
                return true;
            return false;
        }

        // must be flush and straight and
        // be certain cards. No wonder I have
        private bool isRoyalFlush()
        {
            if (isStraight() && isFlush() &&
                  _sortedUp[0].rank == RANK.RA &&
                  _sortedUp[1].rank == RANK.RT &&
                  _sortedUp[2].rank == RANK.RJ &&
                  _sortedUp[3].rank == RANK.RQ &&
                  _sortedUp[4].rank == RANK.RK)
                return true;
            return false;
        }

        private bool isStraightFlush()
        {
            if (isStraight() && isFlush())
                return true;
            return false;
        }

        /*
         * Two choices here, the first four cards
         * must match in rank, or the second four
         * must match in rank. Only because the hand
         * is sorted
         */
        private bool isFourOfAKind()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[2].rank == _sortedUp[3].rank)
                return true;
            if (_sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[2].rank == _sortedUp[3].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
                return true;
            return false;
        }

        /*
         * two choices here, the pair is in the
         * front of the hand or in the back of the
         * hand, because it is sorted
         */
        private bool isFullHouse()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[2].rank == _sortedUp[3].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
                return true;
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
                return true;
            return false;
        }

        /*
         * three choices here, first three cards match
         * middle three cards match or last three cards
         * match
         */
        private bool isThreeOfAKind()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[1].rank == _sortedUp[2].rank)
            {
                HoldBySorted(1, 1, 1, 0, 0);
                return true;
            }
            if (_sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[2].rank == _sortedUp[3].rank)
            {
                HoldBySorted(0, 1, 1, 1, 0);
                return true;
            }
            if (_sortedUp[2].rank == _sortedUp[3].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
            {
                HoldBySorted(0, 0, 1, 1,1);
                return true;
            }
            return false;
        }

        /*
         * three choices, two pair in the front,
         * separated by a single card or
         * two pair in the back
         */
        private bool isTwoPair()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[2].rank == _sortedUp[3].rank)
            {
                HoldBySorted(1, 1, 1, 1, 0);
                return true;
            }
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
            {
                HoldBySorted(1, 1, 0, 1, 1);
                return true;
            }
            if (_sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[3].rank == _sortedUp[4].rank)
            {
                HoldBySorted(0, 1, 1, 1, 1);
                return true;
            }
            return false;
        }

        /*
         * 4 choices here
         */
        private bool isJacksOrBetter()
        {
            if (_sortedUp[0].rank == _sortedUp[1].rank &&
                _sortedUp[0].isJacksOrBetter())
            {
                HoldBySorted(1, 1, 0,0, 0);
                return true;
            }
            if (_sortedUp[1].rank == _sortedUp[2].rank &&
                _sortedUp[1].isJacksOrBetter())
            {
                HoldBySorted(0, 1, 1, 0, 0);
                return true;
            }
            if (_sortedUp[2].rank == _sortedUp[3].rank &&
                _sortedUp[2].isJacksOrBetter())
            {
                HoldBySorted(0, 0, 1, 1, 0);
                return true;
            }
            if (_sortedUp[3].rank == _sortedUp[4].rank &&
                _sortedUp[3].isJacksOrBetter())
            {
                HoldBySorted(0, 0, 0, 1, 1);
                return true;
            }
            return false;
        }


        public JoBmoves GetValidateMove()
        {
            JoBmoves move = GetMove();
            if (_actions.Contains(move.move))
            {
                return move;
            }
            else
            {
                throw new Exception("Logic error");
            }
        }
    }
}