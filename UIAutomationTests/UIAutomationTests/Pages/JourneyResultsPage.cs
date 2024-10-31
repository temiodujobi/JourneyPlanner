using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using UIAutomationTests.Hooks;

namespace UIAutomationTests.Pages
{
    public class JourneyResultsPage : BasePage
    {
        public JourneyResultsPage(BrowserContext context) : base(context)
        {
        }

        private IWebElement EditJourney => Context.Driver.FindElement(By.ClassName("edit-journey"));
        private IWebElement UpdateJourneyButton => Context.Driver.FindElement(By.Id("plan-journey-button"));
        private IWebElement ClearToLocation => Context.Driver.FindElement(By.XPath("//a[contains(text(),'Clear To location')]"));
        private IWebElement ErrorMessage => Context.Driver.FindElement(By.CssSelector("ul.field-validation-errors>li"));
        private IWebElement ExpandPreference => Context.Driver.FindElement(By.CssSelector("button.toggle-options.more-options"));
        private IWebElement PreferenceLeastWalking => Context.Driver.FindElement(By.XPath("//*[@for='JourneyPreference_2']"));
        private IWebElement PreferenceUpdateButton => Context.Driver.FindElement(By.CssSelector("#more-journey-options input.primary-button"));
        private IWebElement JourneyDetails => Context.Driver.FindElement(By.CssSelector("#option-1-content .journey-details"));
        private IWebElement LeastWalkingJourneyTime => Context.Driver.FindElement(By.CssSelector("#option-1-heading .journey-time.no-map"));
        private IWebElement JourneyResultSummaryFrom => Context.Driver.FindElement(By.ClassName("journey-result-summary"));
        private IWebElement JourneyModal => Context.Driver.FindElement(By.CssSelector(".extra-journey-options.multi-modals.clearfix"));
        private IWebElement ViewDetails => Context.Driver.FindElement(By.CssSelector("#option-1-content button.secondary-button.show-detailed-results.view-hide-details"));

        public bool AccessInfoType(string accessInfo) => Context.Driver.FindElement(By.CssSelector($"#option-1-content a.{accessInfo.ToLower().Trim().Replace(" ", "-")}.tooltip-container")).Displayed;

        public string ResultPageHeading => Context.Driver.FindElement(By.CssSelector(".jp-results-headline")).Text; 

        public string InvalidJourneyResultMessage
        {
            get
            {
                WebDriverWait.Until(_ => PageInReadyState && ErrorMessage.Displayed);
                return ErrorMessage.Text;
            }
        }

        public string JourneyTypeTime(string journeyType)
        {
            WebDriverWait.Until(d => PageInReadyState && JourneyModal.Displayed);
            var journeyTime = Context.Driver.FindElement(By.CssSelector($"a.journey-box.{journeyType} .col2.journey-info"));          
            return journeyTime.Text;
        }

        public string JourneyResultSummaryFromText => JourneyResultSummaryFrom.Text;

        public void UpdateJourneyDestination(string to)
        {
            EditJourneyResult(to);
        }
        
        private void EditJourneyResult(string to)
        {
            EditJourney.Click();
            ClearToLocation.Click();
            InputJourneyTo(to);
            UpdateJourneyButton.Click();
        }

        public void SetPreferenceToLeastWalking()
        {
            WebDriverWait.Until(d => PageInReadyState && ExpandPreference.Displayed);
            ExpandPreference.Click();
            WebDriverWait.Until(d => PreferenceLeastWalking.Displayed);
            PreferenceLeastWalking.Click();
            PreferenceUpdateButton.Click();
        }

        public bool GetLeastWalkingJourneyTime()
        {
            WebDriverWait.Until(d => PageInReadyState && JourneyDetails.Displayed);
            return LeastWalkingJourneyTime.Displayed;
        }

        public void ExpandViewDetails()
        {
            WebDriverWait.Until(d => PageInReadyState && ViewDetails.Displayed);
            ViewDetails.Click();
        }
    }
}
