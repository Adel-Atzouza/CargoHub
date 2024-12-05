import requests
import test_helper

url = "http://localhost:3000/api/v2/transfers/"
headers = {'APIKEY': "Admin"}
current_id = 0


def test_post_transfers():
    response = requests.post(url, headers=headers, json=test_helper.transfer_1)
    
    assert response.status_code == 201
    global current_id
    current_id = int(response.text)
    

def test_get_after_post_transfers():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    transfer_json = response.json()
    del transfer_json['created_at']
    del transfer_json['updated_at']
    del transfer_json['transfer_status']

    print(test_helper.transfer_1, transfer_json)

    test_helper.transfer_1['id'] = current_id
    assert test_helper.transfer_1 == transfer_json


def test_get_all_transfers():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200
    
    transfers_json = response.json()

    assert type(transfers_json) is list
    assert 0 <= len(transfers_json) <= 100
    if len(transfers_json) > 0:
        assert test_helper.have_same_structure(test_helper.template_transfer, transfers_json[0])


def test_put():
    test_helper.transfer_1_updated['id'] = current_id
    response = requests.put(url + str(current_id), headers=headers, json=test_helper.transfer_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 200

    transfer_json = response.json()
    del transfer_json['updated_at']
    del transfer_json['created_at']
    del transfer_json['transfer_status']

    test_helper.transfer_1_updated['id'] = current_id
    assert test_helper.transfer_1_updated == transfer_json


def test_delete():
    response = requests.delete(url + str(current_id), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(url + str(current_id), headers=headers)

    assert response.status_code == 404
    assert response.text == "Transfer doesn't exist with id: " + str(current_id)