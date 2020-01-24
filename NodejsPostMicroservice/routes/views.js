
var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';

/* GET all posts */
router.get('/', async (req, res) => {
    console.log("START");
    console.log(req);
    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);
    let collection = db.collection('Posts');

    let result = await collection.find().toArray();


    res.json(result);

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