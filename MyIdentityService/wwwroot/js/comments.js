let name;
let ava;

function create_UUID() {
    var dt = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}

function jsAddComment(CurrUserProfile) {
    let obj = JSON.parse(CurrUserProfile);
    let res = document.querySelector("#text");
    let txt = document.querySelector("#comment");

    name = obj.Name;
    ava = obj.Avatara;

    let uuid = create_UUID();
   
    let temp = `
<li class="media mt-2">
                <div class="media-left">
                    
                        <img class="media-object img-circle" src="/uploads/profiles/100x100.${obj.Avatara}" alt="...">
                    
                </div>
                <div class="media-body">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <div class="author"><b>${obj.Name}</b></div>
                            <div class="metadata">
                                <span class="date">${new Date()}</span>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="media-text text-justify">${txt.value}</div>
                            <div class="pull-right"><a class="btn btn-info" id="${uuid}" onclick="jsReplay('${uuid}')">Replay</a></div>
                              
                        </div>
                        <div id="replayText${uuid}">
                                </div> 
                        
                    </div>
</li>

`
    res.insertAdjacentHTML("beforebegin", temp);
    let tekst = txt.value;
    txt.value = "";

    $.ajax({
        url: '/Post/jsAddComment',
        type: 'POST',
        data: { id: uuid, Obj: CurrUserProfile, Text: tekst},
        success: function (data) {
            //let btn = $('#like');
            //if (btn.hasClass('btn-danger')) {
            //    btn.removeClass('btn-danger').addClass('btn-success');
            console.log('ok');
        }
    });
   
}

function jsReplay(uuid) {
  
    var element = document.getElementById(uuid);
    element.classList.add("hide");
    let rep = document.getElementById("replayText"+uuid); //document.querySelector("#replayText${uuid}");

    let temp = ` 
        <div class="form-group mt-3 pl-3">
            <label for="comment" id="ComTxt">Comment:</label>
            <textarea class="form-control" rows="5" id="RepComment"></textarea>
        </div>
<div class="pl-3 pb-3">
        <button id="AddComment${uuid}" type="button" class="btn btn-info" onclick="jsAddReplayComment('${uuid}')"> Send </button>
</div>
<ul class="media-list">
    <div id="comm${uuid}">
    </div>
</ul>
`;
    rep.insertAdjacentHTML("beforebegin", temp);
   
}

function jsAddReplayComment(uuid) {

    var el = document.getElementById(uuid);
    el.classList.remove("hide");

    var element = document.getElementById("RepComment");
   // element.classList.add("hide");
    let resp = document.getElementById("comm" + uuid); //document.querySelector("#comm");
    let newUuid = create_UUID();

    let temp = ` 
        <li class="media mt-2">
                <div class="media-left">

                        <img class="media-object img-circle" src="/uploads/profiles/100x100.${ava}" alt="...">
                    
                </div>
                <div class="media-body">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <div class="author"><b>${name}</b></div>
                            <div class="metadata">
                                <span class="date">${new Date()}</span>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="media-text text-justify">${element.value}</div>
                            <div class="pull-right"><a class="btn btn-info" id="${newUuid}" onclick="jsReplay('${newUuid}')">Replay</a></div>
                                
                        </div>
<div id="replayText${newUuid}">
                                </div>
                    </div>
</li>

`;
    element.remove();
    document.getElementById("ComTxt").remove();
    document.getElementById("AddComment" + uuid).remove();
    resp.insertAdjacentHTML("beforebegin", temp);

}



