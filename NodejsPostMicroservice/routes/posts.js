var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';



/* GET posts by user id */
router.get('/:id', async (req, res) => {
    let id = req.params.id;
    if (id) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        let dbClient = await client.connect();
        let db = dbClient.db(dbName);
        let collection = db.collection('Posts');

        let result = await collection.find({ ProfileId : id }).toArray();
        

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
    console.log("start update");
    //console.log(item.Description);
    //console.log(item.Id );

    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);

    
    let collection = db.collection('Posts');

    var myquery = { Id: item.Id };

    console.log(item.Method);
    if (item.Method == 'Add') {
        console.log(item.Id);
        var newvalues = {
            $push: { LikesProfileId: item.UserId }
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
                LikesProfileId: [ item.UserId ]}
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
   
//});

// POST /
router.post('/', (req, res) => {
   
    if (req.body.Image && req.body.Title && req.body.ProfileId) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        client.connect(async (err, resp) => {
            if (err) {
                throw new Error(err);
            }

            let db = client.db(dbName);
            let coll = db.collection('Posts');

            let item = req.body;

            let response = await coll.insertOne(item);
            res.sendStatus(200);
            
        });

   }
});

// DELETE post by id
router.delete('/:id', (req, res) => {
    console.log("start delete");
    let id = req.params.id;
    
    console.log(id);
    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    client.connect(async (err, resp) => {
        if (err) {
            throw new Error(err);
        }
   // let dbClient = await client.connect();
    let db = client.db(dbName);
    let collection = db.collection('Posts');

    var myquery = { Id: id };
    collection.deleteOne(myquery, function (err, obj) {
        if (err) throw err;
        console.log("1 document deleted");
        res.sendStatus(200);
    });
    });

});


module.exports = router;