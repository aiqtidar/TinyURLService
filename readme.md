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
	- .Net Version: 8.0


Problems:
	- As you may have noticed, the hashes are computed to determine the uniqueness of a URL. There are a number of problems behind this when scaled:
		- There will be hash collissions for large data volumes.
		- The hashes are inherently 32 bit integers, and can hold up to 2^32 unique values. By comparison, IPV6 addresses have 128 bit addresses. As a result, the number of potential addresses stored in this manner might not be enough in the future.




