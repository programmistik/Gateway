let name;

function jsAddComment(CurrUserProfile) {
    let obj = JSON.parse(CurrUserProfile);
    let res = document.querySelector("#text");
    let txt = document.querySelector("#comment");

    name = obj.Name;
   
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
                            <div class="pull-right"><a class="btn btn-info" id="replayBtn" onclick="jsReplay()">Replay</a></div>
                                <div id="replayText">
                                </div>
                        </div>
                    </div>
</li>

`
    res.insertAdjacentHTML("beforebegin", temp);
    
    txt.value = "";
   
}

function jsReplay() {
  
    var element = document.getElementById("replayBtn");
    element.classList.add("hide");
    let rep = document.querySelector("#replayText");

    let temp = ` 
        <div class="form-group mt-3">
            <label for="comment" id="ComTxt">Comment:</label>
            <textarea class="form-control" rows="5" id="RepComment"></textarea>
        </div>
        <button id="AddComment" type="button" class="btn btn-info" onclick="jsAddReplayComment()"> Send </button>
<ul class="media-list">
    <div id="comm">
    </div>
</ul>
`;
    rep.insertAdjacentHTML("beforebegin", temp);
   
}

function jsAddReplayComment() {

    var element = document.getElementById("RepComment");
   // element.classList.add("hide");
    let resp = document.querySelector("#comm");

    let temp = ` 
        <li class="media mt-2">
                <div class="media-left">

                        <img class="media-object img-circle" src="/uploads/profiles/100x100." alt="...">
                    
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
                            <div class="pull-right"><a class="btn btn-info" id="replayBtn" onclick="jsReplay()">Replay</a></div>
                                <div id="replayText">
                                </div>
                        </div>
                    </div>
</li>
<ul class="media-list">
    <div id="text">
    </div>
</ul>
`;
    element.remove();
    document.getElementById("ComTxt").remove();
    document.getElementById("AddComment").remove();
    resp.insertAdjacentHTML("beforebegin", temp);

}



