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

//unity interaction
showTestPopup = function (id) {
    if (testConfig["State"] == false) {
        getTestInfo(id);
    } else {
        alert("Вы уже проходите тестирование");
    }
};

//gameObject subsystem
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


//test subsystem
var testConfig = [];
testConfig["State"] = false;
testConfig["Id"] = 0;

getTestInfo = function (id) {
    Request("getTestInfo", { id: id }, function (data) {
        if (data.success) {
            testConfig["Id"] = id;
            testConfig["QuestionsCount"] = data.questionsCount;

            $('#TestHeader').text(data.title + " (" + data.topic + ")");

            $('#TestStartTitle').text(data.title);
            $('#TestStartTopic').text(data.topic);
            $('#TestStartAuthor').text(data.author);
            $('#TestStartQuestionsCount').text(data.questionsCount);
            $('#TestStartResultsCount').text(data.resultsCount);
            $('#TestStartDate').text((data.resultsCount > 0) ? data.dateLastResult : "-");
            $('#TestStartOpened').text((data.isOpened) ? "Да" : "Нет");
            $('#TestMainQuestionCount').text(data.questionsCount);

            $('#TestStart').show();
            $('#TestMain').hide();
            $('#TestResult').hide();

            openPopup("#TestPopup");
        } else {
            alert(data.message);
        }
    });
}

startTest = function (id) {
    Request("startTest", { id: id }, function (data) {
        if (data.success) {
            testConfig["State"] = true;
            testConfig["SessionId"] = data.testSessionId;
            testConfig["QuestionsLeft"] = data.questionLeft;
            testConfig["CurrentQuestion"] = testConfig["QuestionsCount"] - testConfig["QuestionsLeft"] + 1;

            getTestQuestion(testConfig["SessionId"], testConfig["CurrentQuestion"]);

            $('#TestStart').hide();
            $('#TestMain').show();
            $('#TestResult').hide();
        } else {
            alert(data.message);
        }
    });
}

getTestQuestion = function (sessionId, questionNumber) {
    Request("getTestQuestion", { testSessionId: sessionId, questionNumber: questionNumber }, function (data) {
        if (data.success) {
            $('#TestMainQuestionNumber').text(data.number);
            $('#TestMainText').text(data.text); // +++++ Application
            testConfig["MultiChoise"] = data.allowMultiChoise;

            $('#TestMainAnswers *').remove();
            data.answers.forEach(function (item) {
                $('#TestMainAnswers').append(
                    $('<li>').append(
                        $('<input>').attr('id', 'question' + item.id).attr('value', item.id).attr('name', 'testAnswer').attr('type', testConfig["MultiChoise"] ? 'checkbox' : 'radio')).append(item.text)
                );
                // ++++++ Application
            });
        } else {
            alert(data.message);
        }
    });
}

setTestAnswer = function (sessionId, questionNumber, answers) {
    Request("setTestAnswer", { testSessionId: sessionId, questionNumber: questionNumber, answersNumbers: answers }, function (data) {
        if (data.success) {
            testConfig["QuestionsLeft"]--;
            testConfig["CurrentQuestion"] = data.questionNext;

            if (data.result == 1) {
                getTestQuestion(testConfig["SessionId"], testConfig["CurrentQuestion"]);
            } else {
                getTestResult(testConfig["SessionId"]);

                $('#TestStart').hide();
                $('#TestMain').hide();
                $('#TestResult').show();

                testConfig["State"] = false;
            }
        } else {
            alert(data.message);
        }
    });
}

getTestResult = function (sessionId) {
    Request("getTestResult", { testSessionId: sessionId }, function (data) {
        if (data.success) {
            $table = $('#TestResultTable');
            data.questionsResults.forEach(function (item) {
                $table.append(
                    $('<tr>')
                        .append($('<td>').text(item.number))
                        .append($('<td>').text(item.result))
                        .append($('<td>').text(item.maxResult))
                );
            });
        } else {
            alert(data.message);
        }
    });
}

$('#TestStartButton').click(function () {
    if ((!testConfig["State"]) && (testConfig["Id"] > 0)) {
        startTest(testConfig["Id"]);
    } else {
        alert("Невозможно начать тест, так как вы уже проходите тестирование или не выбрали ни одного теста.");
    }
});

$('#TestMainButton').click(function () {
    if (!testConfig["State"]) {
        alert('Вы не проходите тестирование.');
        return;
    }

    var $items = $("#TestMainAnswers input[type=" + ((testConfig["MultiChoise"]) ? "checkbox" : "radio") + "]:checked");
    var answersCount = $items.size();
    if (answersCount == 0) {
        alert('Выберите хотя бы один правильный ответ.');
        return;
    }

    var answersArray = new Array();
    $items.each(function (i) {
        answersArray.push($(this).val());
    });

    setTestAnswer(testConfig["SessionId"], testConfig["CurrentQuestion"], answersArray);
});

$(".popup_close, .popup").click(function () {
    testConfig["State"] = false;
});