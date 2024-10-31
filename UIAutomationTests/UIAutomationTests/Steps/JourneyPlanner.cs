using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using UIAutomationTests.Models;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Steps
{
    [Binding]
    public sealed class JourneyPlanner
    {
        private readonly HomePage _homePage;
        private readonly JourneyResultsPage _planJourneyResultPage;

        public JourneyPlanner(HomePage homePage, JourneyResultsPage planJourneyResultPage)
        {
            _homePage = homePage;
            _planJourneyResultPage = planJourneyResultPage;
        }
      
        [Given(@"user is on the TfL home page")]
        public void GivenUserIsOnTheTfLHomePage()
        {
            _homePage.PageTitleText.Should().Be("Keeping London moving - Transport for London");
            _homePage.AcceptCookies();
        }

        [Given(@"user plans a journey from '(.*)' to '(.*)'")]
        [When(@"user plans a journey from '(.*)' to '(.*)'")]
        public void WhenUserPlansAJourneyFromTo(string from, string to)
        {
            _homePage.PlanValidJourney(from, to);
        }
        
        [Given(@"user is presented with the Journey Results")]
        [Then(@"user should be presented with the Journey Results")]
        public void ThenUserShouldBePresentedWithTheJourneyResults(Table table)
        {
            _planJourneyResultPage.ResultPageHeading.Should().Be("Journey results");
            var data = table.CreateInstance<Models.JourneyPlanner>();
            var from = data.From;
            var to = data.To;
            _planJourneyResultPage.JourneyResultSummaryFromText.Should().Contain(from);
            _planJourneyResultPage.JourneyResultSummaryFromText.Should().Contain(to);
        }

        [Then(@"'([^']*)' time should be '([^']*)'")]
        public void ThenJourneyTypeTimeShouldBe(string journeyType, string expectedJourneyTime)
        {
            var actualJourneyTime = _planJourneyResultPage.JourneyTypeTime(journeyType.ToLower());
            expectedJourneyTime.Should().Be(actualJourneyTime);
        }

        [When(@"user plans an invalid journey")]
        public void WhenUserPlansAnInvalidJourney()
        {
            const string to = "9999999";
            const string from = "0000000";
            _homePage.PlanInvalidJourney(to, from);
        }

        [Then(@"user should be presented with the Journey Results page with an error message")]
        public void ThenUserShouldBePresentedWithTheJourneyResultsPageWithAnErrorMessage()
        {
            _planJourneyResultPage.ResultPageHeading.Should().Be("Journey results");
            _planJourneyResultPage.InvalidJourneyResultMessage.Should().Be("Sorry, we can't find a journey matching your criteria");
        }

        [When(@"user tries to plan a journey without locations")]
        public void WhenUserTriesToPlanAJourneyWithoutLocations()
        {
            _homePage.PlanJourneyWithoutLocations();
        }

        [Then(@"user should be presented with required fields error messages")]
        public void ThenUserShouldBePresentedWithRequiredFieldsErrorMessages(Table table)
        {
            var data = table.CreateInstance<Models.JourneyPlanner>();
            var fromErrorMessage = data.From;
            var toErrorMessage = data.To;

            _homePage.InputFromFieldErrorMessage.Should().Be(fromErrorMessage);
            _homePage.InputToFieldErrorMessage.Should().Be(toErrorMessage);
        }

        [When(@"user changes the destination to '(.*)'")]
        public void WhenUserChangesTheDestinationTo(string to)
        {
            _planJourneyResultPage.UpdateJourneyDestination(to);
        }

        [When(@"user checks the recent planned journeys")]
        public void WhenUserChecksTheRecentPlannedJourneys()
        {
            _homePage.Context.Driver.Navigate().Refresh();
            _homePage.NavigateToHomePage();
            _homePage.ClickRecentTab();
        }

        [Then(@"user should be presented with recently planned journeys")]
        public void ThenUserShouldBePresentedWithRecentlyPlannedJourneys(Table table)
        {
            var data = table.CreateInstance<Models.JourneyPlanner>();
            var from = data.From;
            var to = data.To;
            var actualResult = _homePage.RecentlyPlannedJourneys();
            actualResult.Should().Contain(from);
            actualResult.Should().Contain(to);
        }

        [When(@"user updates journey with least walking routes")]
        public void WhenUserUpdatesJourneyWithLeastWalkingRoutes()
        {
            _planJourneyResultPage.SetPreferenceToLeastWalking();
        }

        [Then(@"journey time should be displayed")]
        public void ThenJourneyTimeShouldBeDisplayed()
        {
            _planJourneyResultPage.GetLeastWalkingJourneyTime().Should().BeTrue();
        }

        [Given(@"a journey contains least walking routes between '([^']*)' to '([^']*)'")]
        public void GivenAJourneyContainsLeastWalkingRoutesBetweenTo(string from, string to)
        {
            _homePage.PageTitleText.Should().Be("Keeping London moving - Transport for London");
            _homePage.AcceptCookies();
            _homePage.PlanValidJourney(from, to);
            _planJourneyResultPage.SetPreferenceToLeastWalking();
        }

        [When(@"user views the complete access")]
        public void WhenUserViewsTheCompleteAccess()
        {
            _planJourneyResultPage.ExpandViewDetails();
        }

        [Then(@"user should be presented with the complete access information")]
        public void ThenUserShouldBePresentedWithTheCompleteAccessInformation(Table table)
        {
            var data = table.CreateSet<CompleteAccessInformation>();
            foreach (var item in data) 
            {
                var actualAccessInfo = _planJourneyResultPage.AccessInfoType(item.AccessInformation);
                var expectedAccessInfo = item.IsDisplayed;
                expectedAccessInfo.Should().Be(actualAccessInfo);
            }
        }

    }
}
