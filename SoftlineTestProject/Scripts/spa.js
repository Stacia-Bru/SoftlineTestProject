let NumberOfFileRecords = 0;    //Число записей в файле
let NumberOfCells = 0;          //Число столбцов

function CutExtention(filename) {
    return filename.substr(0, $("#DirectoryDialogOptionName").val().lastIndexOf("."));
}

function fixDirectoryName(dir) {
    if (dir.substr(dir.length - 1) != "\\")
        dir += "\\";
    return dir;
}

function toUnicode(line) {
    var unicodeString = '';
    for (var i = 0; i < line.length; i++) {
        var theUnicode = line.charCodeAt(i).toString(16).toUpperCase();
        while (theUnicode.length < 4) {
            theUnicode = '0' + theUnicode;
        }
        theUnicode = '\\u' + theUnicode;
        unicodeString += theUnicode;
    }
    return unicodeString;
}

function getSeparator() {
    let radios = document.getElementsByName('FileSeparator');
    //console.log(radios);
    for (let i = 0; i < radios.length; i++) {
        if (radios[i].checked)
            switch (radios[i].value) {
                case 'Tab': return '\\u0009'; break;
                case 'Space': return '\\u0020'; break;
                case 'Semicolon': return '\\u003B'; break;
                case 'Custom': return toUnicode($('#FileSeparatorText').val()); break; //закодируем в юникод на всяк пожарный
            }
    }
}



function setCurrent(filename, directory, isDirectory, self) {
    //фиксируем подсветку, чтобы видеть, что мы выбрали
    $(self).addClass('table-active').siblings().removeClass('table-active');

    //Выставляем скрытое поле DirectoryDialogTarget на нужный контролер
    if (isDirectory) {
        $("#DirectoryDialogTarget").val("DirectoryListing"); //выставляем action и div 
        $("#DirectoryDialogOptionName").val(filename);  //запоминаем временное имя следующей директории, текущая директория не сменилась
    } else {
        $("#DirectoryDialogTarget").val("ParseFile"); //выставляем action и div 
        $("#DirectoryDialogOptionName").val(filename);  //запоминаем временное имя файла, текущая директория не сменилась
    }
   // console.log($("#DirectoryDialogTarget").val());
   // console.log($("#DirectoryDialogOptionName").val());
}


function SubmitDirDialogForm() {
    let separator = getSeparator();   //определяем символ для разделения ячеек в файле
    let DirectoryName = fixDirectoryName($("#DirectoryName").val());   //имя директории изменится только после запроса на сервер
    let FileName = $("#DirectoryDialogOptionName").val();  //имя файла берем из скрытого поля

    let isHeader = $('#FirstLineIsHeader').is(":checked");  //определяем надо ли выделять строку под заголовки
    let params = {
        DirectoryName: DirectoryName,
        FileName: FileName,
        separator: separator,
        isHeader: isHeader
    };  //формируем параметры для запроса
    console.log(params);

    //По нажатию кнопки "Открыть"
    let action = $("#DirectoryDialogTarget").val();
    //console.log(path);   //путь к выбранному файлу или директории

    if (DirectoryName.length > 0) {

        //запрос через AJAX
        $.ajax({
            url: "/Home/" + action,
            type: "POST",
            data: params,
            success: function (result) {
                // содержимое ответа от контролера выгружаем в соответствующий блок
                $("#" + action).html(result);
                // если мы получили распарсенный файл - закрываем диалоговое окно выбора файлов
                if (action == "ParseFile") {
                    //выставляем выбранный файл в окно в основной форме, директория еще не сменилась, она меняется только при перерисовке DirectoryListing автоматически
                    $("#FileNameWithoutExt").val(CutExtention(FileName));
                    $("#FileName").val(FileName);
                    $('#myModalBox').modal('hide');
                }
            },
            error: function (result) {
                    $("#DirectoryDialogWarning").html("Файл не может быть октрыт.");
            }
        });
    }

}

function SetHeaderValue(souce, target) {
    $('#'+target).html($('#'+souce).val());
}

function AddRecord() {
    var tableRef = document.getElementById('RecordList');
    var newRow = tableRef.insertRow(window.NumberOfFileRecords);
    for (let i = 0; i < window.NumberOfCells; i++) {
        var newCell = newRow.insertCell(i);
        var newInput = document.createElement('input');
        newInput.type = 'text';
        newInput.className = 'form-control  form-control-tablecell';
        newInput.name = 'RecordCell' + window.NumberOfFileRecords
        newCell.appendChild(newInput);
    }
    window.NumberOfFileRecords++;
}

function readRecordFromForm(id, cellTag, isHeader)
{
    let CellData = new Array();
    let tag = (id) ? cellTag + id : cellTag;
    
    for (let i = 0; i < document.getElementsByName(tag).length; i++)
    {
        CellData.push(document.getElementsByName(tag)[i].value);
    }
    return { "CellData": CellData, "isHeader": isHeader };
}

function readAllRecordsfromForm(path, hasHeader)
{
    let ResultArrayOfRecords = new Array();
    if (hasHeader) {
        //нам нужны заголовки
        ResultArrayOfRecords.push(readRecordFromForm(null, 'HeaderCell', true));
    }
    for (let i = 1; i < window.NumberOfFileRecords; i++) {
        //нам нужна запись из таблицы
        ResultArrayOfRecords.push(readRecordFromForm(i, 'RecordCell', false));
    }
    //console.log(ResultArrayOfRecords);
    return { "ListOfRecords":ResultArrayOfRecords, "Path": path };
}

function Save() {
    let separator = getSeparator();
    let DirectoryName = fixDirectoryName($('#DirectoryName').val());
    let FileName = $('#FileName').val();
    let path = DirectoryName + FileName;
    let hasHeader = $('#FirstLineIsHeader').is(":checked");
    let params = {
        path: path,
        separator: separator,
        recordSet: readAllRecordsfromForm(DirectoryName + FileName, hasHeader)
    };
    console.log(params);

    //По нажатию кнопки "Сохранить"
    let action = "SaveFile";  //контролер

    if (action.length > 0) {
        //запрос через AJAX
        $.ajax({
            url: "/Home/" + action,
            type: "POST",
            data: params,
            success: function (result) { $('#SaveFileWarning').html(result); },
            error: function (result, ajaxOptions, thrownError) {$('#SaveFileWarning').html("Не удалось сохранить файл.");}
        });
    }
}
