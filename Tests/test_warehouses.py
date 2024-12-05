import requests
import test_helper

url = "http://localhost:3000/api/v2/warehouses/"
headers = {'APIKEY': "Admin"}
current_id = 0


def test_post_warehouses():
    response = requests.post(url, headers=headers, json=test_helper.warehouse_1)
    
    assert response.status_code == 201
    global current_id
    current_id = int(response.text)
    

def test_get_after_post_warehouses():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    warehouse_json = response.json()
    del warehouse_json['created_at']
    del warehouse_json['updated_at']

    test_helper.warehouse_1['id'] = current_id
    assert test_helper.warehouse_1 == warehouse_json


def test_get_all_warehouses():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200
    
    warehouses_json = response.json()

    assert type(warehouses_json) is list
    assert 0 <= len(warehouses_json) <= 100
    if len(warehouses_json) > 0:
        assert test_helper.have_same_structure(test_helper.template_warehouse, warehouses_json[0])


def test_put():
    test_helper.warehouse_1_updated['id'] = current_id
    response = requests.put(url + str(current_id), headers=headers, json=test_helper.warehouse_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    warehouse_json = response.json()
    del warehouse_json['updated_at']
    del warehouse_json['created_at']

    test_helper.warehouse_1_updated['id'] = current_id
    assert test_helper.warehouse_1_updated == warehouse_json


def test_delete():
    response = requests.delete(url + str(current_id), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 404
    assert response.text == "Warehouse doesn't exist with id: " + str(current_id)


# NOTES: 
# Updating warehouse without 'created_at' field, removes the field.
# This means that the whole structure can be changed by the 'put' method.
# All fields can be changed, even ID which shouldn't be possible

# new tests that should be made:
# 1 - Test that ID can't be changed.
# 2 - Test that fields can't be altered / added
# 3 - Test that values from fields like "created_at" / "updated_at" can't be manualy altered.