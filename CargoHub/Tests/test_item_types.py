import requests
import test_helper

url = "http://localhost:3000/itemtypes/"
headers = {'API_KEY': "a1b2c3d4e5"}


def test_get_random_item_types():
    response = requests.get(url + str(1), headers=headers)

    assert response.status_code == 200
    assert test_helper.have_same_structure(
        test_helper.template_item_type, response.json())


def test_get_all_item_types():
    response = requests.get(url[:-1], headers=headers)

    assert response.status_code == 200

    item_types_json = response.json()

    assert type(item_types_json) is list
    assert len(item_types_json) > 0
    assert test_helper.have_same_structure(
        test_helper.template_item_type, item_types_json[0])


def test_post_item_types():
    response = requests.post(url, headers=headers,
                             json=test_helper.item_type_1)

    assert response.status_code == 201


def test_get_after_post_item_types():
    response = requests.get(
        url + str(test_helper.item_type_1['id']), headers=headers)

    assert response.status_code == 200


def test_put():
    response = requests.put(
        url + str(test_helper.item_type_1['id']), headers=headers, json=test_helper.item_type_1_updated)

    assert response.status_code == 200


def test_get_after_put():
    response = requests.get(
        url + str(test_helper.item_type_1_updated['id']), headers=headers)

    assert response.status_code == 200

    item_type_json = response.json()
    del item_type_json['updated_at']
    del item_type_json["created_at"]

    assert test_helper.item_type_1_updated == item_type_json


def test_delete():
    response = requests.delete(
        url + str(test_helper.item_type_1_updated['id']), headers=headers)

    assert response.status_code == 200


def test_get_after_delete():
    response = requests.get(
        url + str(test_helper.item_type_1_updated['id']), headers=headers)

    assert response.status_code == 404
