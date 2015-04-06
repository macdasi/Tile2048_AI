using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.BlackJack;

namespace WebApplication1.Logic
{
    public class BlackJackLogic
    {
        private Actions[] _actions;
        private List<Card> _deck;
        private List<Card> _player = new List<Card>();
        private List<Card> _dealer = new List<Card>();

        private int CalcSum(List<Card> hand)
        {
            int sum = hand.Where(x => x.rank > (RANK)1).Select(x => (int)x.Value).Sum();
            if (sum <= 21)
            {
                int aces = hand.Count(x => x.rank == RANK.RA);
                if (aces > 0)
                {
                    sum = sum + (aces * 11);
                    while(sum > 21 && aces > 0) {
                        aces--;
                        sum -= 10;
                    }
                }
                return sum;
            }
            else
            {
                return sum;
            }
        }

        private Card FromString(string cardStr)
        {
            _deck = Deck.NewDeck();

            RANK rank = (RANK) Enum.Parse(typeof(RANK), "R"+cardStr.Split('_')[1]);        
            SUIT suit = (SUIT) Enum.Parse(typeof(SUIT), cardStr.Split('_')[0]);  
            return new Card(rank,suit);
        }

        public BlackJackLogic(Actions[] actions, string[] player, string[] dealer) 
        {
            _player = new List<Card>();
            _dealer = new List<Card>();

            _actions = actions;
            foreach (var item in player)
            {
                _player.Add(FromString(item));
            }
            foreach (var item in dealer)
            {
                _dealer.Add(FromString(item));
                break;
            }
        }

        public Actions GetMove() 
        {
            if (_actions.Count() == 1)
            {
                return _actions[0];
            }
            else if (_actions.Count() > 1)
            {
                #region You have an ace and...
                if (_player.Any(x => x.rank == RANK.RA) && _player.Count() == 2)
                {
                    var secondCard = _player.Where(x => x.rank != RANK.RA).FirstOrDefault();
                    RANK r = secondCard == null ? RANK.RA : secondCard.rank;

                    switch (r)
                    {
                        case RANK.RA: return Actions.SPLIT;
                        case RANK.R2:
                        case RANK.R3:
                            if (_dealer.Any(x=>x.rank == RANK.R5 || x.rank == RANK.R6))
                            {
                                return Actions.DOUBLE;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R4:
                        case RANK.R5:
                            if (_dealer.Any(x => x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6))
                            {
                                return Actions.DOUBLE;
                            }
                            else
                            {
                                return Actions.HIT;
                            }

                        case RANK.R6:
                            if (_dealer.Any(x => x.rank == RANK.R3 || x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6))
                            {
                                return Actions.DOUBLE;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R7:
                            if (_dealer.Any(x => x.rank == RANK.R3 || x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6))
                            {
                                return Actions.DOUBLE;
                            }
                            else if (_dealer.Any(x => x.rank == RANK.R2 || x.rank == RANK.R7 || x.rank == RANK.R8))
                            {
                                return Actions.STAND;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R8:
                        case RANK.R9:
                        case RANK.RT:
                        case RANK.RJ:
                        case RANK.RQ:
                        case RANK.RK:
                            return Actions.STAND;
                        default:
                            throw new Exception("Logic error");
                    }


                }
                #endregion

                #region You have a pair of...

                if (_player.Count == 2 && _player[0].rank == _player[1].rank)
                {
                    switch (_player[0].rank)
                    {
                        case RANK.RA:return Actions.STAND;
                        case RANK.R2:
                        case RANK.R3:
                            if (_dealer.Any(x => x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6 || x.rank == RANK.R7))
                            {
                                return Actions.SPLIT;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R4:
                            return Actions.HIT;
                        case RANK.R5:
                            if (_dealer.Any(x => x.rank >= RANK.R2 && x.rank <= RANK.R9))
                            {
                                return Actions.DOUBLE;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R6:
                            if (_dealer.Any(x => x.rank == RANK.R3 || x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6 ))
                            {
                                return Actions.SPLIT;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R7:
                            if (_dealer.Any(x => x.rank >= RANK.R2 && x.rank <= RANK.R7))
                            {
                                return Actions.SPLIT;
                            }
                            else
                            {
                                return Actions.HIT;
                            }
                        case RANK.R8:
                            return Actions.SPLIT;
                        case RANK.R9:
                            if (_dealer.Any(x => x.rank >= RANK.R2 && x.rank <= RANK.R6))
                            {
                                return Actions.SPLIT;
                            }
                            else if (_dealer.Any(x => x.rank == RANK.R7))
                            {
                                return Actions.STAND;
                            }
                            else if (_dealer.Any(x => x.rank == RANK.R8 || x.rank == RANK.R9))
                            {
                                return Actions.SPLIT;
                            }
                            else
                            {
                                return Actions.STAND;
                            }
                        case RANK.RT:
                        case RANK.RJ:
                        case RANK.RQ:
                        case RANK.RK:
                            return Actions.STAND;
                        default:
                            throw new Exception("Logic error");
                    }
                }

                #endregion

                #region or You have...

                if (_player.Count > 0)
                {
                    int sum = CalcSum(_player);
                    if (sum >= 21)
                    {
                        return Actions.BET;
                    }
                    else if (sum <= 8)
                    {
                        return Actions.HIT;
                    }
                    else if (sum == 9)
                    {
                        if (_dealer.Any(x => x.rank == RANK.R3 || x.rank == RANK.R4 || x.rank == RANK.R5 || x.rank == RANK.R6))
                        {
                            return Actions.DOUBLE;
                        }
                        else
                        {
                            return Actions.HIT;
                        }
                    }
                    else if (sum == 10)
                    {
                        if (_dealer.Any(x => x.rank >= RANK.R2 && x.rank <= RANK.R9))
                        {
                            return Actions.DOUBLE;
                        }
                        else
                        {
                            return Actions.HIT;
                        }
                    }
                    else if (sum == 11)
                    {
                        if (_dealer.Any(x => x.rank == RANK.RA))
                        {
                            return Actions.HIT;
                        }
                        else
                        {
                            return Actions.DOUBLE;
                        }
                    }
                    else if (sum == 12)
                    {
                        if (_dealer.Any(x => x.rank >= RANK.R4 && x.rank <= RANK.R6))
                        {
                            return Actions.STAND;
                        }
                        else
                        {
                            return Actions.HIT;
                        }
                    }
                    else if (sum >= 13 && sum <= 16)
                    {
                        if (_dealer.Any(x => x.rank >= RANK.R2 && x.rank <= RANK.R6))
                        {
                            return Actions.STAND;
                        }
                        else
                        {
                            return Actions.HIT;
                        }
                    }
                    else if (sum >= 17 && sum <= 21)
                    {
                        return Actions.STAND;
                    }
                }

                #endregion
            }
            throw new Exception("Logic error");
        }

        public int GetValidateMove()
        {
            Actions move = GetMove();
            if (_actions.Contains(move))
            {
                return (int)move;
            }
            else
            {
                throw new Exception("Logic error");
            }
        }
    }
}