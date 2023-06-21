using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PyramidSolitaire;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GeneralPlaymodeTests
{
    private IEnumerator Init()
    {
        yield return SceneManager.LoadSceneAsync("MainScene");
        yield return null;
        yield return null;
        yield return null;
        yield return new WaitForSeconds(2);
    }

    [UnityTest, Repeat(30), Timeout(int.MaxValue)]
    public IEnumerator ExecuteGameLoop()
    {
        yield return Init();

        var drawPile = CardPile.Get<CardPileStack>(CardPosition.DrawPile);
        var discardPile = CardPile.Get<CardPileStack>(CardPosition.DiscardPile);
        var pyramidPile = CardPile.Get<CardPilePyramid>(CardPosition.Pyramid);

        var gameManager = Object.FindObjectOfType<GameManager>();
        var interactionSystem = Object.FindObjectOfType<InteractionSystem>();

        EndState? endState = null;

        gameManager.OnGameOver += state => endState = state;

        // Adding a max loop count just in case... 👀
        int maxLoopCount = 200;
        while (!endState.HasValue && --maxLoopCount > 0)
        {
            // Select cards which values can sum to 13
            int count  = GameAutoMatcher.GetMatchingCards(pyramidPile, discardPile, out var matchingCards);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var card = matchingCards[i];
                    interactionSystem.ClickOnCard(card);
                }
            }
            else
            {
                // if no match is available, try pick from draw pile
                if (!drawPile.TryPeek(out var drawTopCard))
                    Assert.Fail("No matches and no cards to draw. Gameover should supposed to called at this point");

                interactionSystem.ClickOnCard(drawTopCard);
            }

            yield return new WaitForSeconds(0.25F);
        }

        Debug.Log($"EndState = {endState!.Value}");
        if (endState!.Value == EndState.Lost)
        {
            Assert.That(drawPile.Count, Is.Zero);
            Assert.That(pyramidPile.Cards, Is.Not.Empty);
        }
        else
        {
            Assert.That(pyramidPile.Cards, Is.Empty);
        }

        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator ClickCardDrawPile_CardMoveToDiscard()
    {
        yield return Init();

        var drawPile = CardPile.Get<CardPileStack>(CardPosition.DrawPile);
        var discardPile = CardPile.Get<CardPileStack>(CardPosition.DiscardPile);
        InteractionSystem interactionSystem = Object.FindObjectOfType<InteractionSystem>();

        drawPile.TryPeek(out var drawTopCard);
        interactionSystem.ClickOnCard(drawTopCard);
        discardPile.TryPeek(out var discardTopCard);

        Assert.AreSame(drawTopCard, discardTopCard);
    }
}