 <div id="top"></div>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/StevePeters-US/Unity-Raspberry-Pi">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">Unity Raspberry Pi</h3>

  <p align="center">
    The intent of this project is to train an ai agent in Unity's ml agents that can control a real world robotic car.
    <br />
    <a href="https://github.com/StevePeters-US/Unity-Raspberry-Pi"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/StevePeters-US/Unity-Raspberry-Pi">View Demo</a>
    ·
    <a href="https://github.com/StevePeters-US/Unity-Raspberry-Pi/issues">Report Bug</a>
    ·
    <a href="https://github.com/StevePeters-US/Unity-Raspberry-Pi/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)
The intent of this project is to create a low cost robot which can be trained and controlled from Unity instance on a seperate machine.

This project started as an elegoo ardiuino car kit. You can find a similar kit with just the chassis, wheels, and motors for about $20. I ended up using the elegoo ln298 motor controller which came with the kit, which can also be found for a few bucks.

I'm using a pi zero on the car which isn't particularly powerful, but it is cheap ($15) and has built in wifi. Add a $5 buck converter and a $15 and we end up with a total hardware cost under $100.

Since the pi is fairly slow we want to keep as much computation done remotely as possible. We can do this by hosting a python websocket server on the pi which controls the GPIO pins. We can access this server in Unity with websockets plus

which doesn't have enough processing power to run inference, so it has to be controlled remotely. To do this 
I set up a simple web socket server on the pi which can be accessed with websockets plus in Unity c#.


<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

* [Unity](https://unity.com/)
* [Python](https://www.python.org/)
* [Raspberry Pi](https://www.raspberrypi.org/)
* [Fusion 360](https://www.autodesk.com/products/fusion-360/overview)
* [Cura](https://ultimaker.com/software/ultimaker-cura)
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

[Here](https://www.youtube.com/watch?v=13HnJPstnDM) is a great video explaining how Unity can interact with websockets

### Prerequisites

Install websockets sharp in Unity
* npm
  ```sh
  npm install npm@latest -g
  ```

### Installation

1. [Set the pi ip address to a static address](https://howchoo.com/pi/configure-static-ip-address-raspberry-pi#:~:text=How%20to%20Configure%20a%20Static%20IP%20Address%20on,...%205%20Test%20the%20static%20IP%20address.%20)

2.  [Set the camera streaming server and GPIO control server to run on start with a chron job](https://www.bc-robotics.com/tutorials/setting-cron-job-raspberry-pi/)

3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `config.js`
   ```js
   const API_KEY = 'ENTER YOUR API';
   ```

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [] Feature 1
- [] Feature 2
- [] Feature 3
    - [] Nested Feature

See the [open issues](https://github.com/StevePeters-US/Unity-Raspberry-Pi/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

[@Steve04](https://twitter.com/@Steve04)

[https://github.com/StevePeters-US/Unity-Raspberry-Pi](https://github.com/StevePeters-US/Unity-Raspberry-Pi)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* []()
* []()
* []()

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/StevePeters-US/Unity-Raspberry-Pi.svg?style=for-the-badge
[contributors-url]: https://github.com/StevePeters-US/Unity-Raspberry-Pi/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/StevePeters-US/Unity-Raspberry-Pi.svg?style=for-the-badge
[forks-url]: https://github.com/StevePeters-US/Unity-Raspberry-Pi/network/members
[stars-shield]: https://img.shields.io/github/stars/StevePeters-US/Unity-Raspberry-Pi.svg?style=for-the-badge
[stars-url]: https://github.com/StevePeters-US/Unity-Raspberry-Pi/stargazers
[issues-shield]: https://img.shields.io/github/issues/StevePeters-US/Unity-Raspberry-Pi.svg?style=for-the-badge
[issues-url]: https://github.com/StevePeters-US/Unity-Raspberry-Pi/issues
[license-shield]: https://img.shields.io/github/license/StevePeters-US/Unity-Raspberry-Pi.svg?style=for-the-badge
[license-url]: https://github.com/StevePeters-US/Unity-Raspberry-Pi/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/stevempeters
[product-screenshot]: images/PiCar.jpg

 
