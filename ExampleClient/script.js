// A variable with our URL
const URL = "http://localhost:8000/";

// A function to fetch our array of tasks(our data)
const getData = () => {
    fetch(URL)
    .then(response => response.json())
    .then(data => {
        console.log(data);
        data.forEach(task => appendItem(task))
    });
}

const appendItem = (task) => {
    //grab our container
    let container = document.getElementById("taskContainer");
    let div = document.createElement("div");
    let header = document.createElement("h3");
    let p = document.createElement("p");

    div.id = task.Id;
    header.innerText = task.Title;
    p.innerText = task.Description;

    div.appendChild(header);
    div.appendChild(p);

    container.appendChild(div);
}