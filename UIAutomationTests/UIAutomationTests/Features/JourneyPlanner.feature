@JourneyPlanner
Feature: JourneyPlanner

Scenario: 1. Plan a valid journey between 2 stations
	Given user is on the TfL home page
	When user plans a journey from 'Leicester Square' to 'Covent Garden'
	Then user should be presented with the Journey Results
		| From                                 | To                                |
		| Leicester Square Underground Station | Covent Garden Underground Station |
	And 'cycling' time should be '1mins'
	And 'walking' time should be '6mins'

Scenario: 2. Update journey with least walking routes
	Given user is on the TfL home page
	And user plans a journey from 'Leicester Square' to 'Covent Garden'
	When user updates journey with least walking routes
	Then journey time should be displayed

Scenario: 3. Verify complete access information
	Given a journey contains least walking routes between 'Leicester Square' to 'Covent Garden'
	When user views the complete access 
	Then user should be presented with the complete access information
	| AccessInformation | IsDisplayed |
	| Up Stairs         | true        |
	| Up Lift           | true        |
	| Level Walkway     | true        |

 Scenario: 4. Invalid Journey
	Given user is on the TfL home page
	When user plans an invalid journey
	Then user should be presented with the Journey Results page with an error message

Scenario: 5. Display error messages when no locations are entered
	Given user is on the TfL home page
	When user tries to plan a journey without locations
	Then user should be presented with required fields error messages
	| From                        | To                        |
	| The From field is required. | The To field is required. |


# Additional Scenarios
@ignore
Scenario: 6. Edit a journey for a different destination
	Given user is on the TfL home page
	And user plans a journey from 'Leicester Square' to 'Covent Garden'
	When user changes the destination to 'London Victoria'
	Then user should be presented with the Journey Results
		| From             | To              |
		| Leicester Square | London Victoria |

@ignore
Scenario: 7. Check recently planned journeys
	Given user is on the TfL home page
	And user plans a journey from 'Leicester Square' to 'Covent Garden'
	And user is presented with the Journey Results
		| From                                 | To                                |
		| Leicester Square Underground Station | Covent Garden Underground Station |
	When user checks the recent planned journeys
	Then user should be presented with recently planned journeys
		| From                                 | To                                |
		| Leicester Square Underground Station | Covent Garden Underground Station |

@ignore
Scenario: 8. Plan a valid journey for a future date
	Given user is on the TfL home page
	When user plans a journey from 'Leicester Square' to 'Covent Garden'
	And user set the day to 'Fri 29 Nov' and time to '12:00'
	Then user should be presented with the Journey Results
		| From                                 | To                                |
		| Leicester Square Underground Station | Covent Garden Underground Station |
	And Leaving 'Friday 29th Nov, 12:00'

@ignore
Scenario: 9. Favourite a planned  journey 
	Given user is on the TfL home page
	When user plans a journey from 'Leicester Square' to 'Covent Garden'
	Then user should be presented with the Journey Results
		| From                                 | To                                |
		| Leicester Square Underground Station | Covent Garden Underground Station |
	And user should add the journey to favourites 
	And the journey should be listed on favourite recents page

@ignore
Scenario: 10. Save new journey preferences
	Given user is on the TfL home page
	When user plans a journey from 'Leicester Square' to 'Covent Garden'
	And user updates journey with routes with fewer changes
	And user saves preferences for future visits
	Then preferences should be saved for future journey searches