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
<ul class="media-list">
    <div id="text">
    </div>
</ul>
`
    res.insertAdjacentHTML("beforebegin", temp);
    
    txt.value = "";
   
}

function jsReplay() {
    let replayBtn = $("#replayBtn");
    let rep = document.querySelector("#replayText");
    replayBtn.addClass('hide');

    let temp = ` 
        <div class="form-group mt-3">
            <label for="comment">Comment:</label>
            <textarea class="form-control" rows="5" id="comment"></textarea>
        </div>
        <button id="AddComment" type="button" class="btn btn-info" onclick="jsAddComment()"> Send </button>
`;
    rep.insertAdjacentHTML("beforebegin", temp);
    replayBtn.removeClass('hide');
    rep.addClass('hide');
}

