﻿/* globals describe it */

var expect = require('chai').expect,
    sinon = require('sinon'),
    DataSet = require('../src/js/data/data-set.js');

describe('DataSet tests', function () {
    describe('Initialization', function () {
        it('Expect default initialization to not throw', function () {
            expect(function () {
                new DataSet();
            }).to.not.throw();
        });

        it('Expect default initialization to return DataSet object with empty data array', function () {
            var dataSet = new DataSet();
            expect(dataSet).to.have.property('data');
            expect(Array.isArray(dataSet.data)).to.be.equal(true);
            expect(dataSet.data.length).to.be.equal(0);
        });

        it('Expect default initialization to return DataSet object with add function', function () {
            var dataSet = new DataSet();
            expect(dataSet).to.have.property('add');
            expect(dataSet.add).to.be.a('function');
        });

        it('Expect default initialization to return DataSet object with addMulti function', function () {
            var dataSet = new DataSet();
            expect(dataSet).to.have.property('addMulti');
            expect(dataSet.addMulti).to.be.a('function');
        });

        it('Expect default initialization to return DataSet object with remove function', function () {
            var dataSet = new DataSet();
            expect(dataSet).to.have.property('remove');
            expect(dataSet.remove).to.be.a('function');
        });

        it('Expect default initialization to return DataSet object with removeAll function', function () {
            var dataSet = new DataSet();
            expect(dataSet).to.have.property('removeAll');
            expect(dataSet.removeAll).to.be.a('function');
        });
    });

    describe('Add items to DataSet', function () {
        it('Expect addition of single valid item to DataSet to add it correctly with valid id', function () {
            var item = {
                a: 1,
                getHash: () => 1
            };

            var spy = sinon.spy(item, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item);
            }).to.not.throw();

            expect(spy.callCount).to.be.equal(0);

            expect(dataSet.data.length).to.equal(1);

            var addedItem = dataSet.data[0];
            expect(addedItem.a).to.equal(item.a);

            expect(addedItem).to.have.property('id');
        });

        it('Expect addition of two valid items with different hash to DataSet to add them correctly with valid id', function () {
            var item1 = {
                    a: 1,
                    getHash: () => 1
                },
                item2 = {
                    a: 2,
                    getHash: () => 2
                };

            var spy1 = sinon.spy(item1, 'getHash'),
                spy2 = sinon.spy(item2, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.not.throw();

            expect(function () {
                dataSet.add(item2);
            }).to.not.throw();

            expect(spy1.callCount).to.be.equal(1);
            expect(spy2.callCount).to.be.equal(1);

            expect(dataSet.data.length).to.equal(2);

            var addedItem1 = dataSet.data[0];
            expect(addedItem1.a).to.equal(item1.a);

            var addedItem2 = dataSet.data[1];
            expect(addedItem2.a).to.equal(item2.a);

            expect(addedItem1).to.have.property('id');
            expect(addedItem2).to.have.property('id');

            expect(addedItem1.id).not.to.be.equal(addedItem2.id);
        });

        it('Expect addition of three valid items with different hash to DataSet to add them correctly with valid id', function () {
            var item1 = {
                    a: 1,
                    getHash: () => 1
                },
                item2 = {
                    a: 2,
                    getHash: () => 2
                },
                item3 = {
                    a: 3,
                    getHash: () => 3
                };

            var spy1 = sinon.spy(item1, 'getHash'),
                spy2 = sinon.spy(item2, 'getHash'),
                spy3 = sinon.spy(item3, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.not.throw();

            expect(function () {
                dataSet.add(item2);
            }).to.not.throw();

            expect(function () {
                dataSet.add(item3);
            }).to.not.throw();

            expect(spy1.callCount).to.be.equal(2);
            expect(spy2.callCount).to.be.equal(2);
            expect(spy3.callCount).to.be.equal(1);

            expect(dataSet.data.length).to.equal(3);

            var addedItem1 = dataSet.data[0];
            expect(addedItem1.a).to.equal(item1.a);

            var addedItem2 = dataSet.data[1];
            expect(addedItem2.a).to.equal(item2.a);

            var addedItem3 = dataSet.data[2];
            expect(addedItem3.a).to.equal(item3.a);

            expect(addedItem1).to.have.property('id');
            expect(addedItem2).to.have.property('id');
            expect(addedItem3).to.have.property('id');

            expect(addedItem1.id).not.to.be.equal(addedItem2.id);
            expect(addedItem1.id).not.to.be.equal(addedItem3.id);
        });

        it('Expect addition of two valid items with equal hash to DataSet to add only the first one', function () {
            var item1 = {
                    a: 1,
                    getHash: () => 1
                },
                item2 = {
                    a: 2,
                    getHash: () => 1
                };

            var spy1 = sinon.spy(item1, 'getHash'),
                spy2 = sinon.spy(item2, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.not.throw();

            expect(function () {
                dataSet.add(item2);
            }).to.not.throw();

            expect(spy1.callCount).to.be.equal(1);
            expect(spy2.callCount).to.be.equal(1);

            expect(dataSet.data.length).to.equal(1);

            var addedItem = dataSet.data[0];
            expect(addedItem.a).to.equal(item1.a);

            expect(addedItem).to.have.property('id');
        });

        it('Expect addition of single item with no getHash() to DataSet to throw and not to add it', function () {
            var item = {
                a: 1
            };

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item);
            }).to.throw('Item to add should have function "getHash"');

            expect(dataSet.data.length).to.equal(0);
        });

        it('Expect addition of one valid item and one item with no getHash() to DataSet to throw and to add only the first one', function () {
            var item1 = {
                    a: 1,
                    getHash: () => 1
                },
                item2 = {
                    a: 2
                };

            var spy1 = sinon.spy(item1, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.not.throw();

            expect(function () {
                dataSet.add(item2);
            }).to.throw('Item to add should have function "getHash"');

            expect(spy1.callCount).to.be.equal(0);

            expect(dataSet.data.length).to.equal(1);

            var addedItem = dataSet.data[0];
            expect(addedItem.a).to.equal(item1.a);

            expect(addedItem).to.have.property('id');
        });

        it('Expect addition of one item with no getHash() and one valid item to DataSet to throw and to add only the second one', function () {
            var item1 = {
                    a: 1
                },
                item2 = {
                    a: 2,
                    getHash: () => 2
                };

            var spy2 = sinon.spy(item2, 'getHash');

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.throw('Item to add should have function "getHash"');

            expect(function () {
                dataSet.add(item2);
            }).to.not.throw();

            expect(spy2.callCount).to.be.equal(0);

            expect(dataSet.data.length).to.equal(1);

            var addedItem = dataSet.data[0];
            expect(addedItem.a).to.equal(item2.a);

            expect(addedItem).to.have.property('id');
        });

        it('Expect addition of two items with no getHash() to DataSet to throw and not to add them', function () {
            var item1 = {
                    a: 1
                },
                item2 = {
                    a: 2
                };

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item1);
            }).to.throw('Item to add should have function "getHash"');

            expect(function () {
                dataSet.add(item2);
            }).to.throw('Item to add should have function "getHash"');

            expect(dataSet.data.length).to.equal(0);
        });

        it('Expect addition of null item to DataSet to not throw and to not add id', function () {
            var item = null;

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item);
            }).to.not.throw();

            expect(dataSet.data.length).to.equal(0);
        });

        it('Expect addition of undefined item to DataSet to not throw and to not add id', function () {
            var item = undefined;

            var dataSet = new DataSet();

            expect(function () {
                dataSet.add(item);
            }).to.not.throw();

            expect(dataSet.data.length).to.equal(0);
        });

        it('Expect add() with no parameters to empty DataSet to not throw and to not change data.length', function () {
            var dataSet = new DataSet();

            expect(function () {
                dataSet.add();
            }).to.not.throw();

            expect(dataSet.data.length).to.equal(0);
        });

        it('Expect add() with no parameters to not throw and to not change data.length', function () {
            var dataSet = new DataSet();

            // First add some items
            dataSet.add({
                x: 1,
                getHash: () => 1
            });

            expect(function () {
                dataSet.add();
            }).to.not.throw();

            expect(dataSet.data.length).to.equal(1);
        });
    });
});