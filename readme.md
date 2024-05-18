Problem: Write a POC for a tinyURL service


Design Requirements:
- Not a web service - (no controller layer)
- Input output should use the command line
- No persistent storage layer required - just in memory for now but should be designed in a way to accomodate for it



Business Requirements:

- CREATE a short URLs with associated long URLs:
	- CREATE a random URL
	- CREATE from a custom short URL
- DELETE short URLs with associated long URLs
- GET long URL from a short URL
- GET number of times a short URL has been clicked
 


Constraints:

- Short URLs must be unique
- 


Approach:

- We'll assume this will be built into a Web Service once the POC has passed. This means
	- We spend least amount of time on the Command Line UI
	- Keep the architecture simple but sustainable, so that code can be reused in the future
	
- Specs:
	- Architecture: MVVM
	- .Net Version: 8.0






