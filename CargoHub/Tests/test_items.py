import requests
import test_helper

url = "http://localhost:3000/items/"
headers = {'API_KEY': "a1b2c3d4e5"}
current_id = 0


def test_get_random_items():
    response = requests.get(url + "P000001", headers=headers)

    assert response.status_code == 200
    print(response.json())
    assert test_helper.have_same_structure(
        test_helper.template_item, response.json())


def test_get_all_items():
    response = requests.get(url, headers=headers)

    assert response.status_code == 200

    items_json = response.json()

    assert type(items_json) is list
    assert len(items_json) > 0
    assert test_helper.have_same_structure(
        test_helper.template_item, items_json[0])


def test_post_items():
    response = requests.post(url, headers=headers, json=test_helper.item_1)

    assert response.status_code == 201


def test_get_after_post_items():
    response = requests.get(
        url + str(test_helper.item_1['uid']), headers=headers)

    assert response.status_code == 200

    item_json = response.json()
    del item_json['created_at']
    del item_json['updated_at']

    assert test_helper.item_1 == item_json


def test_put():
    response = requests.put(
        url + str(test_helper.item_1['uid']), headers=headers, json=test_helper.item_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(
        url + str(test_helper.item_1_updated['uid']), headers=headers)

    assert response.status_code == 200

    item_json = response.json()
    del item_json['updated_at']
    del item_json["created_at"]

    assert test_helper.item_1_updated == item_json


def test_delete():
    response = requests.delete(
        url + str(test_helper.item_1_updated['uid']), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(
        url + str(test_helper.item_1_updated['uid']), headers=headers)

    assert response.status_code == 404
