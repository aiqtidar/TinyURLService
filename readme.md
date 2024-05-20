# TinyURL Service
A simple service, written with .NET 8, for generating unique tiny URLs for a provided URL.

Note:
- The service is intended to be a simple console application.
- This project is not intended to be a web service (no controller layer!)
- There is no persistent memory, although one can be provisioned and added as a module easily. 

## What this project includes:
- Use of simple Dependency Injection and code modularity through interfaces in C#
- Understanding of how to structure projects and build maintainable and scalable code
- Use of Abstract Base Classes and inheritance
- Use of Generics
- Use of Configuration building (partially implemented)
- Use of various interfaces, such as the IComparer Interface for generating consistent equality and hash checks
- Understanding of Unit Testing

## Functionality
- CREATE:
	- CREATE a random URL
	- CREATE from a custom short URL
- GET:
	- A tiny URL of an URL from memory
	- A URL from a Tiny URL from memory
	- A tiny URL's popularity (number of hits) from memory
- DELETE:
	- A tiny URL
	- A URL (and all its associated tiny URLs)

Note that there are a number of functions available in the API that are not accessible from the UI (check InMemoryRepository.cs for example)

## Approach:

A simple approach would be to introduce two Dictionaries for bidirectional mapping between URLs and TinyURLs. I chose not to go with this approach.

Instead, I chose to implement a Trie data structure for storing URLs, with each Node contains the TinyURLs for each URL. There are several advantages to this:
	- The Root Node always contains all domains, follows by port and path. This ensures that domains, ports and path are grouped together.
	- Generating statistics on URLs is much simpler and take much less time complexity. For example, one can find the number of Tiny URLs for a domain, port or any of its paths in linear time.
	- Measures of similarity between URLs can be established by simply comparing height of the each URL's tree.
	- In a business setting, vendors would have the option to purchase quantities of domains, ports or paths and this data structure would separate the data between vendors based on that (as well as generate individual statistics for each vendor).

### Potential Problems with this approach:
As you may have noticed, the hashes are computed to determine the uniqueness of a URL. There are a number of problems behind this when scaled:
	- There will be hash collissions for large data volumes.
	- The hashes are inherently 32 bit integers, and can hold up to 2^32 unique values. By comparison, IPV6 addresses have 128 bit addresses. As a result, the number of potential addresses stored in this manner might not be enough in the future.

## TODO
As with all good projects, there is always more to do!

I've left a number of TODOs in the codebase, so that I can improve it in the future when I (if ever) find time. **Please understand that this project was completed within 7 hours and concessions were made to complete it within the time constaints I had.**


