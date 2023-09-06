# Reddit Statistics Application

![GitHub repo size](https://img.shields.io/github/repo-size/tgibson1/redditCodeChallenge)
![GitHub license](https://img.shields.io/github/license/tgibson1/redditCodeChallenge)

This .NET 6/7 application listens to a selected subreddit in near real-time, collecting and tracking various statistics. It continuously consumes data from Reddit's REST APIs, adheres to rate limiting, and processes posts concurrently to ensure high throughput without blocking. The application calculates and reports the following statistics:

1. Posts with the most upvotes.
2. Users with the most posts.

The application keeps all data in-memory and does not require a database. It also considers scalability for handling multiple subreddits in the future. The code follows SOLID principles for maintainability and loose coupling to external systems and dependencies. It includes error handling and unit testing, making it production-ready.

## Table of Contents

- [Features](#features)
- [Installation and Usage](#installation-and-usage)
- [Application Structure](#application-structure)
- [Contributing](#contributing)
- [License](#license)

## Features

- Real-time tracking of statistics from a chosen subreddit.
- Efficiently utilizes Reddit's REST APIs, considering rate limiting.
- Concurrent processing to maximize computing resources.
- Scalable for handling multiple subreddits.
- Follows SOLID principles for maintainable and extensible code.
- Includes error handling for robustness.
- Unit testing ensures code quality.
- Keeps all data in-memory without the need for a database.

## Installation and Usage

1. Clone the repository to your local machine.

   ```bash
   git clone https://github.com/tgibson1/redditCodeChallenge.git
