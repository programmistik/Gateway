function jsAddComment(CurrUserProfile) {
    let obj = JSON.parse(CurrUserProfile);
    let res = document.querySelector("#text");
    let txt = document.querySelector("#comment");

    let temp = `
<li class="media mt-2">
                <div class="media-left">
                    <a>
                        <img class="media-object img-circle" src="/uploads/profiles/100x100.${obj.Avatara}" alt="...">
                    </a>
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
                            <div class="pull-right"><a class="btn btn-info" href="#">Replay</a></div>
                        </div>
                    </div>
</li>
`
    res.insertAdjacentHTML("beforebegin", temp);
    
    txt.value = "";
   
}

