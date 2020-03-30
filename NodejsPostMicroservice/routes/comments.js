var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
//const url = 'mongodb+srv://diana:Unicorn123@cluster0-nhtbf.mongodb.net/test?retryWrites=true&w=majority';

const dbName = 'InstaApp';





// PUT /Change post
router.put('/', async (req, res) => {
    let item = req.body;
    console.log("start update");
    //console.log(item.Description);
    //console.log(item.Id );

    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);


    let collection = db.collection('Posts');

    var myquery = { Id: item.Id };

   
        console.log(item.Id);
        var newvalues = {
            $push: { Comments: item.Comment }
        };

        collection.updateOne(myquery, newvalues, function (err, resp) {
            if (err) throw err;
            console.log("1 comment added");
            res.sendStatus(200);
        });
   
    //collection.deleteOne({ Id: item.id });


    //let response = await collection.insertOne(item);
    //res.sendStatus(200);


});

//});



module.exports = router;