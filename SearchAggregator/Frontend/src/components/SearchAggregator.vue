<template>
  <div class="search">
    <h1>Аггрегатор поисковых запросов Google Bing Yandex</h1>
      <form @submit.prevent>
        <input class="search__input" v-model="searchInput"/>
        <button type="submit" class="search__button" @click="searchWebResults()">Поиск</button>
      </form>
      <!--<div class="search__wrapper">
        <h2>Google</h2>
        <h2>Yandex</h2>
        <h2>Bing</h2>
      </div>-->
      <p class="search__preloader" v-if="isDataLoading">Загружаем данные...</p>
      <section class="search__section section" v-else>
      <article class="section__article">
        <ul class="section__list">
          <li class="section__item item" v-for="searchEntity in searchData.searchResults">
            <div class="item__wrapper">
                <h3 class="item__title">{{ Object.values(searchEntity)[1] }}</h3>
                <a class="item__link" :href="searchEntity.url" target="_blank">{{Object.values(searchEntity)[2] }}</a>
                <p class="item__description">{{ Object.values(searchEntity)[3] }}</p>
            </div>
          </li>
        </ul>
      </article>
    </section>
  </div>
</template>

<script>
import axios from 'axios'
export default {
  name: 'SearchAggregator',
  data() {
    return {
      searchInput: "",
      isDataLoading: false,
      searchData: {
        searchResults: [],
      }
    }
  },
  methods: {
    searchWebResults() {
      this.isDataLoading = true;
      axios.get(`http://localhost:5133/api/v1/aggregator/search?searchText=${this.searchInput}`).then(response => {
          this.searchData.searchResults = response.data;
          this.isDataLoading = false;
      }).catch(error => {
        console.log(error);
        this.isDataLoading = false;
      })
    }
  }
}
</script>

<style scoped>
  .search__section {
    display: flex;
    flex-direction: row;
    justify-content: space-evenly;
  }
  
  .section__list {
    list-style-type: none;
  }

  .section_list:not(:last-of-type) {
    margin-bottom: 10px;
  }
  .item__link {
    overflow-x: hidden;
  }

  .item__wrapper {
    display: flex;
    flex-direction: column;
  }

  .section__article{
    width:30%;
  }

  .item__description {
    text-align: justify;
  }

  .search__wrapper {
    display: flex;
    justify-content: space-around;
    width: 100%;
  }

  .search__button {
    width: 70px;
    height: 35px;
    font-size: 17px;
  }

  .search__input{
    width: 300px;
    height: 29px;
    font-size: 17px;
    margin-right: 5px;
  }

  .search__preloader {
    font-size: 25px;
  }
</style>