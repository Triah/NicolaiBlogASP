function directhome() {
    window.location.href = "http://localhost:1621/home";
}

function directarticles() {
    window.location.href = "http://localhost:1621/articles";
}

function directproducts() {
    window.location.href = "http://localhost:1621/products";
}

function directabout() {
    window.location.href = "http://localhost:1621/about";
}

function directcontact() {
    window.location.href = "http://localhost:1621/contact";
}

function directlogin() {
    window.location.href = "http://localhost:1621/login";
}


/* When the user clicks on the button, 
toggle between hiding and showing the dropdown content */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {

    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}