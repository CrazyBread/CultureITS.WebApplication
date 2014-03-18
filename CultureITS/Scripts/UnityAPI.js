//main function
Request = function (command, data, result) {
    $.ajax({
        type: "POST",
        url: "/Api/" + command,
        data: { data: JSON.stringify(data) },
        traditional: true,
        success: result
    });
};

//command function
markGameObject = function (id) {
    Request("markGameObject", { id: id }, function (data) {
        if (data.success)
            alert('ok');
        else
            alert(data.message);
    });
};

checkGameObject = function (id) {
    Request("checkGameObject", { id: id }, function (data) {
        if (data.success)
            alert(data.state);
        else
            alert(data.message);
    });
};

getGameObjectInfo = function (id, full) {
    Request("getGameObjectInfo", { id: id, full: full }, function (data) {
        if (data.success)
            alert(data.name + " " + data.canNotified + " " + data.description);
        else
            alert(data.message);
    });
};

arrayTesting = function () {
    Request("arrayTesting", { array: [{ a: 1, b: "2" }, { a: 16, b: "hello" }, { a: -1, b: "51" }] }, function (data) {
        if (data.success)
            alert("cool");
        else
            alert(data.message);
    });
};