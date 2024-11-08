import requests
import test_helper

url = "http://localhost:3000/api/v1/transfers"
headers = {'API_KEY': "a1b2c3d4e5"}


def test_get_random_transfers():
    response = requests.get(url + str(119240), headers=headers)
    
    assert response.status_code == 200
    assert test_helper.have_same_structure(test_helper.template_transfer, response.json())


def test_get_all_transfers():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200
    
    transfers_json = response.json()

    assert type(transfers_json) is list
    assert len(transfers_json) > 0
    assert test_helper.have_same_structure(test_helper.template_transfer, transfers_json[0])


def test_post_transfers():
    response = requests.post(url, headers=headers, json=test_helper.transfer_1)
    
    assert response.status_code == 201
    

def test_get_after_post_transfers():
    response = requests.get(url + str(test_helper.transfer_1['id']), headers=headers)

    assert response.status_code == 200

    transfer_json = response.json()
    del transfer_json['created_at']
    del transfer_json['updated_at']
    del transfer_json['transfer_status']

    print(test_helper.transfer_1, transfer_json)

    assert test_helper.transfer_1 == transfer_json


def test_put():
    response = requests.put(url + str(test_helper.transfer_1['id']), headers=headers, json=test_helper.transfer_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(url + str(test_helper.transfer_1_updated['id']), headers=headers)

    assert response.status_code == 200

    transfer_json = response.json()
    del transfer_json['updated_at']

    assert test_helper.transfer_1_updated == transfer_json


def test_delete():
    response = requests.delete(url + str(test_helper.transfer_1_updated['id']), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(url + str(test_helper.transfer_1_updated['id']), headers=headers)

    assert response.status_code == 200
    assert response.content == b'null'


# NOTES: 
# On the server the field transfer_status gets added

