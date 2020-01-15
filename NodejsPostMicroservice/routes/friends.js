var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';



// PUT /Change post
router.put('/', async (req, res) => {
    let item = req.body;
    console.log("start friends update");
    //console.log(item.Description);
    //console.log(item.Id );

    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);


    let collection = db.collection('Profiles');

    var myquery = { AppUserId : item.Id };

    console.log(item.Method);
    if (item.Method == 'Add') {
        console.log(item.Id);
        var newvalues = {
            $push: { Friends: item.UserId }
        };

        collection.updateOne(myquery, newvalues, function (err, resp) {
            if (err) throw err;
            console.log("1 document updated");
            res.sendStatus(200);
        });
    }
    else {
        var newvalues = {
            $pullAll: {
                Friends: [item.UserId]
            }
        };

        collection.updateOne(myquery, newvalues, function (err, resp) {
            if (err) throw err;
            console.log("1 document deleted");
            res.sendStatus(200);
        });

    }
    //collection.deleteOne({ Id: item.id });


    //let response = await collection.insertOne(item);
    //res.sendStatus(200);


});
module.exports = router;