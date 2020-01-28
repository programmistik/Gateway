
var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';

/* GET all new posts from my friends */
router.get('/:id/:myId', async (req, res) => {
    let id = req.params.id;
    let myId = req.params.myId;
    //console.log("START view update");
    //console.log(id);
    //console.log(myId);
    if (id) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        let dbClient = await client.connect();
        let db = dbClient.db(dbName);
        let collection = db.collection('Posts');
        //console.log(collection);
        //let all = await collection.find({ ProfileId: id }).toArray();
        //console.log(all);
        let result = await collection.find(
            { ViewsProfileId: { $ne: myId }}
        ).toArray();

        res.json(result);
    } else {
        res.statusCode = 404;
        res.statusMessage = 'Post not found!';
        let error = {
            code: res.statusCode,
            message: res.statusMessage
        };
        res.json(error);
    }
});

// PUT /Change post
router.put('/', async (req, res) => {
    let item = req.body;
    console.log("start view update");
    //console.log(item.Description);
    //console.log(item.Id );

    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);


    let collection = db.collection('Posts');

    var myquery = { Id: item.Id };

   
    
        var newvalues = {
            $push: { ViewsProfileId: item.UserId }
        };

        collection.updateOne(myquery, newvalues, function (err, resp) {
            if (err) throw err;
            console.log("1 document updated");
            res.sendStatus(200);
        });
   
    


});

module.exports = router;