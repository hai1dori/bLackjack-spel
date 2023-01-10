//Class diagremet :
//  Deck
//    -- Shuffle()
//    -- DealCard()
//  Player
//    -- Hand
//      -- AddCard()
//      -- GetScore()
//    -- Stay()
//    -- Hit()
//    -- IsBust()
//  Game
//    -- Play()

using System;
using System.Collections.Generic;

namespace Blackjack
{
    class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            
            //Gör en kortlek med alla 52 kort
            for (int suit = 0; suit < 4; suit++)
            {
                for (int value = 1; value < 14; value++)
                {
                    cards.Add(new Card { Suit = (Suit)suit, Value = value });
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            //Fisher-Yates shuffle
            //Kod från google :/
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                Card temp = cards[swapIndex];
                cards[swapIndex] = cards[i];
                cards[i] = temp;
                
            }
        }

        public Card DealCard()
            //tar kortet som är längst fram i listan eftersom hela listan e shuffled
        {
            Card topCard = cards[0];
            cards.RemoveAt(0);
            return topCard;
        }
    }

    class Player
    {
        public List<Card> Hand { get; set; }
        public bool IsBust { get; set; }

        public Player()
        {
            Hand = new List<Card>();
            IsBust = false;
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }

        public int GetScore()
            //Pallade inte med A huvudvärk så den får vara 10
        {
            int score = 0;
            foreach (Card card in Hand)
            {
                if (card.Value > 10)
                {
                    score += 10;
                }
                else
                {
                    score += card.Value;
                }
            }
            return score;
        }

     
        public void Stay()
        {
            //gör inget allstå stand
        }

        public void Hit(Card card)
            //Hit alltså ge ett till kort
        {
            Hand.Add(card);
            if (GetScore() > 21)
            {
                IsBust = true;
            }
        }
    }

    class Game
    {
        public Player Player { get; set; }
        public Player Dealer { get; set; }
        public Deck Deck { get; set; }

        public Game()
        {
            Player = new Player();
            Dealer = new Player();
            Deck = new Deck();
        }

        public void Play()
        {
            Deck.Shuffle();
            //Ge två kort till varje spelare
            Player.AddCard(Deck.DealCard());
            Dealer.AddCard(Deck.DealCard());
            Player.AddCard(Deck.DealCard());
            Dealer.AddCard(Deck.DealCard());
            //Kolla om nån har blackjack
            if (Player.GetScore() == 21)
            {
                Console.WriteLine("Blackjack! You win!");
                return;
            }
            else if (Dealer.GetScore() == 21)
            {
                Console.WriteLine("Dealer has blackjack. You lose!");
                return;
            }
            //Ska spelaren Hit eller Stay
            while (!Player.IsBust)
            {
                Console.WriteLine("Your score: " + Player.GetScore());
                Console.WriteLine("Hit or stay (h/s)?");
                string input = Console.ReadLine();

                if (input == "h")
                {
                    Player.Hit(Deck.DealCard());
                }
                else if (input == "s")
                {
                    Player.Stay();
                    break;
                }
            }
            //Om spelaren går över/bust är spelet över
            if (Player.IsBust)
            {
                Console.WriteLine("You are bust. You lose!");
                return;
            }
            //Annars går den till dealer
            //extremt smart dealer AI aktiveras
            while (Dealer.GetScore() < 17)
            {
                Dealer.Hit(Deck.DealCard());
            }
            //Kolla vem som är närmast 21
            //Resterande harakät
            int playerScore = Player.GetScore();
            int dealerScore = Dealer.GetScore();
            if (dealerScore > 21)
            {
                Console.WriteLine("Dealer is bust. You win!");
            }
            else if (playerScore > dealerScore)
            {
                Console.WriteLine("Your score: " + playerScore);
                Console.WriteLine("Dealer score: " + dealerScore);
                Console.WriteLine("You win!");
            }
            else
            {

                Console.WriteLine("Your score: " + playerScore);
                Console.WriteLine("Dealer score: " + dealerScore);
                Console.WriteLine("You lose!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }

    enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    class Card
    {
        public Suit Suit { get; set; }
        public int Value { get; set; }
    }
}